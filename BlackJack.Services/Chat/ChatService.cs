namespace BlackJack.Services.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class ChatServiceFactory : DuplexServiceFactory<ChatService> {}

    public class ChatService : DuplexService
    {
        //Base class (DuplexService) keeps track of all connected chatters
        //But it only deals with Session IDs, it has no concept of "chat nickname"
        //So this dictionary will map Session IDs to Nicknames:
        Dictionary<string,string> chatters = new Dictionary<string,string>();

        Timer stockTimer;
        // MasterQuotes is a Dictionary of stock symbols and a list of client sessionIds that are subscribed to that stock
        private Dictionary<string, List<string>> MasterQuotes = new Dictionary<string, List<String>>();

        public ChatService()
        {
            //Set up a stock update every 30 seconds
            this.stockTimer = new Timer(new TimerCallback(this.StockUpdate),null, 0, 30000);
        }

        //This is the modified StockUpdate method that now sends out realtime quotes to each subscriber
        void StockUpdate(object o)
        {
            // iterate all the symbols we have stored for clients
            foreach (string symbol in this.MasterQuotes.Keys)
            {
                StockTickerMessage stm = new StockTickerMessage();
                // get the list of sessionIds that are subscribed to this stock symbol...
                var clients = this.MasterQuotes[symbol];
                stm.stock = symbol;
                // get the quote for this stock (can also do multiple symbols but keeping it simple for now)
                var quotes = QuoteUtility.GetQuotes(new string[] {stm.stock});
                stm.price = Decimal.Parse(quotes[0].LastTrade);
                stm.LastTradeTime = quotes[0].Time;
                stm.Change = quotes[0].Change;
                stm.High = quotes[0].High;
                stm.Open = quotes[0].Open;
                stm.Low = quotes[0].Low;
                // send out the stock update message to all the clients subscribed to this symbol
                this.PushToSelectedClients(stm, clients);
            }
        }

        protected override void OnMessage(string sessionId, DuplexMessage data)
        {
            if (data is JoinChatMessage)
            {
                //If a chatter joined, let all other chatters know
                JoinChatMessage msg = (JoinChatMessage)data;
                if (!this.chatters.ContainsValue(msg.nickname))
                {
                    this.chatters.Add(sessionId, msg.nickname);
                    this.PushToAllClients(data);
                }
            }
            else if(data is LeaveChatMessage)
            {
                this.OnDisconnected(sessionId);
            }
            else if (data is TextChatMessageToServer)
            {
                //If a chatter sent a message, broadcast it to all other chatters
                TextChatMessageToServer msg = (TextChatMessageToServer)data;
                // Check for a stock subscription---
                if (msg.text.ToLower().Contains("subscribe"))
                {
                    string symbol = msg.text.Split(' ')[1];
                    symbol = symbol.ToUpper();
                    // is the symbol already there? If not, add it:
                    if(!this.MasterQuotes.ContainsKey( symbol))
                    {
                        this.MasterQuotes.Add(symbol, new List<string> {sessionId });
                    }
                    else
                    {
                        //sessionIds is List<String> containing the sessionIds subscribed to this stock symbol
                        var sessionIds = this.MasterQuotes[symbol];
                        sessionIds.Add(sessionId);
                    }
                }
                else
                {
                    TextChatMessageFromServer outMsg = new TextChatMessageFromServer();
                    outMsg.text = msg.text;
                    outMsg.textColor = msg.textColor;
                    //Incoming chat message does not have the chatter's nickname, so we add it
                    outMsg.nickname = this.chatters[sessionId];
                    this.PushToAllClients(outMsg);
                }
            }
        }

        protected override void OnDisconnected(string sessionId)
        {
            //If a chatter disconnected, let all other chatters know
            string nickname;
            if (this.chatters.TryGetValue(sessionId, out nickname))
            {
                LeaveChatMessage lcm = new LeaveChatMessage();
                lcm.nickname = nickname;
                this.PushToAllClients(lcm);
                this.chatters.Remove(sessionId);
                // we could also unsubscribe from all his stock symbols here
            }
        }
    }
}