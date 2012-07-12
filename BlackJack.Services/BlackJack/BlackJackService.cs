namespace BlackJack.Services.BlackJack
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using global::BlackJack.Services.BlackJack.Objects;

    using XmlWriter = global::BlackJack.Services.BlackJack.XML.XmlWriter;

    public class BlackJackServiceFactory : DuplexServiceFactory<BlackJackService> { }

    public class BlackJackService : DuplexService
    {
        //Base class (DuplexService) keeps track of all connected chatters
        //But it only deals with Session IDs, it has no concept of "chat nickname"
        //So this dictionary will map Session IDs to Nicknames:
        Dictionary<string,string> players = new Dictionary<string,string>();
        List<string> dealRequests = new List<string>();
        List<string> disconnectRequests = new List<string>();
        XmlWriter writer = new XmlWriter();
        Deck deck = new Deck();

        public BlackJackService()
        {
            
        }

        protected override void OnMessage(string sessionId, DuplexMessage data)
        {
            if (data is JoinGameMessageToServer)
            {
                JoinGameMessageToServer msg = (JoinGameMessageToServer)data;  
                if(this.players.ContainsValue(msg.nickname))
                {
                    PlayerAlreadyExistsMessageFromServer outMsg = new PlayerAlreadyExistsMessageFromServer();
                    this.PushMessageToClient(sessionId, outMsg);
                }
                else
                {
                    this.players.Add(sessionId, msg.nickname);
                    JoinGameMessageFromServer outMsg = new JoinGameMessageFromServer();
                    outMsg.playerId = msg.playerId;
                    outMsg.nickname = this.players[sessionId];
                    string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.Load(filename);
                        outMsg.xmlDoc = xmlDoc.InnerXml;
                    }
                    catch (System.IO.FileNotFoundException)
                    {

                    }
                    this.PushMessageToClient(sessionId, outMsg);
                }
            }
            else if (data is LeaveGameMessageToServer)
            {
                
            }
            else if (data is AddPlayerMessageToServer)
            {
                AddPlayerMessageToServer msg = (AddPlayerMessageToServer) data;

                AddPlayerMessageFromServer outMsg = new AddPlayerMessageFromServer();
                outMsg.playerId = msg.playerId;
                outMsg.nickname = this.players[sessionId];
                this.PushToAllClients(outMsg);
                this.writer.PlayerAdded(msg);
            }
            else if (data is RemovePlayerMessageToServer)
            {
                RemovePlayerMessageToServer msg = (RemovePlayerMessageToServer)data;

                RemovePlayerMessageFromServer outMsg = new RemovePlayerMessageFromServer();
                outMsg.playerId = msg.playerId;
                outMsg.nickname = this.players[sessionId];
                this.PushToAllClients(outMsg);
                this.writer.PlayerRemoved(msg);

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                if (currentPlayers.Count == 0)
                {
                    ClearDealerMessageFromServer clearMsg = new ClearDealerMessageFromServer();
                    this.PushToAllClients(clearMsg);
                    this.writer.DealerRemove();
                }

            }
            else if (data is BetMessageToServer)
            {
                BetMessageToServer msg = (BetMessageToServer)data;

                BetMessageFromServer outMsg = new BetMessageFromServer();
                outMsg.betAmount = msg.betAmount;
                outMsg.playerId = msg.playerId;
                outMsg.nickname = this.players[sessionId];
                this.PushToAllClients(outMsg);
                this.writer.PlayerBet(msg);
            }
            else if (data is DealMessageToServer)
            {
                DealMessageToServer msg = (DealMessageToServer)data;

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                List<string> currentActivePlayers = new List<string>();
                foreach (XmlNode player in currentPlayers)
                {
                    if(!this.dealRequests.Contains(player.Attributes["PlayerName"].Value))
                        currentActivePlayers.Add(player.Attributes["PlayerName"].Value);
                }

                this.dealRequests.Add(msg.nickname);
                bool shouldDeal = false;
                foreach (var player in currentActivePlayers)
                {
                    shouldDeal = this.dealRequests.Contains(player);
                    if (!shouldDeal)
                        break;
                }
                if (shouldDeal)
                {
                    DealMessageFromServer outMsg = new DealMessageFromServer();
                    this.deck = new Deck();
                    outMsg.deck = this.deck;
                    outMsg.nickname = this.players[sessionId];
                    this.PushToAllClients(outMsg);
                    this.writer.DealerRemove();
                    this.writer.DealCards(this.deck);
                    this.dealRequests.Clear();
                }
            }
            else if (data is FinishedDealingMessageToServer)
            {
                FinishedDealingMessageToServer msg = (FinishedDealingMessageToServer)data;

                FinishedDealingMessageFromServer outMsg = new FinishedDealingMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                //outMsg.timer = new Timer(10000);
                this.PushToAllClients(outMsg);
            }
            else if (data is StandMessageToServer)
            {
                StandMessageToServer msg = (StandMessageToServer)data;

                StandMessageFromServer outMsg = new StandMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                outMsg.playerId = msg.playerId;
                this.PushToAllClients(outMsg);
            }
            else if (data is HitMessageToServer)
            {
                HitMessageToServer msg = (HitMessageToServer)data;

                HitMessageFromServer outMsg = new HitMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                outMsg.playerId = msg.playerId;
                this.PushToAllClients(outMsg);
                this.writer.PlayerHit(msg, this.deck);
            }
            else if (data is DoubleMessageToServer)
            {
                DoubleMessageToServer msg = (DoubleMessageToServer)data;

                DoubleMessageFromServer outMsg = new DoubleMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                outMsg.playerId = msg.playerId;
                this.PushToAllClients(outMsg);
                this.writer.PlayerDouble(msg, this.deck);
            }
            else if (data is SplitMessageToServer)
            {
                SplitMessageToServer msg = (SplitMessageToServer)data;

                SplitMessageFromServer outMsg = new SplitMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                outMsg.playerId = msg.playerId;
                this.PushToAllClients(outMsg);
                this.writer.PlayerSplit(msg, this.deck);
            }
            else if (data is ClearPlayersMessageToServer)
            {
                ClearPlayersMessageToServer msg = (ClearPlayersMessageToServer)data;

                ClearPlayersMessageFromServer outMsg = new ClearPlayersMessageFromServer();
                outMsg.nickname = this.players[sessionId];
                this.PushToAllClients(outMsg);
                this.writer.PlayersRemoved(msg);
                
            }
        }

        protected override void OnDisconnected(string sessionId)
        {
            string nickname;
            if (this.players.TryGetValue(sessionId, out nickname))
            {
                LeaveGameMessageFromServer lcm = new LeaveGameMessageFromServer();
                lcm.nickname = nickname;
                this.PushToAllClients(lcm);
                this.players.Remove(sessionId);
                this.writer.PlayersRemoved(lcm);

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                if (currentPlayers.Count == 0)
                {
                    ClearDealerMessageFromServer clearMsg = new ClearDealerMessageFromServer();
                    this.PushToAllClients(clearMsg);
                    this.writer.DealerRemove();
                }
            }
        }
    }
}