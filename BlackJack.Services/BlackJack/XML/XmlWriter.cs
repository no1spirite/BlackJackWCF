namespace BlackJack.Services.BlackJack.XML
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Web.Hosting;
    using System.Xml;

    using global::BlackJack.Services.BlackJack.Objects;

    public class XmlWriter
    {
        ReaderWriterLock rwl = new ReaderWriterLock();
        public XmlWriter()
        {
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(filename);
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Table");
                xmlWriter.WriteStartElement("Players");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Dealer");
                xmlWriter.WriteStartElement("DealerCards");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Close();
                xmlDoc.Load(filename);
            }
        }

        public void PlayerAdded(AddPlayerMessageToServer msg)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode players = root.FirstChild;
            XmlElement player = xmlDoc.CreateElement("Player");
            XmlElement hands = xmlDoc.CreateElement("Hands");
            XmlElement hand = xmlDoc.CreateElement("Hand");
            XmlElement cards = xmlDoc.CreateElement("Cards");
            XmlElement chips = xmlDoc.CreateElement("Chips");
            player.SetAttribute("PlayerId", msg.playerId.ToString());
            player.SetAttribute("PlayerName", msg.nickname);
            hand.SetAttribute("HandId", "1");
            hand.AppendChild(cards);
            hand.AppendChild(chips);
            hands.AppendChild(hand);
            player.AppendChild(hands);
            players.AppendChild(player);
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayerRemoved(RemovePlayerMessageToServer msg)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode players = root.FirstChild;
            XmlNode player = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']");
            player.ParentNode.RemoveChild(player);
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayersRemoved(LeaveGameMessageFromServer msg)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNodeList deletedPlayers = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerName='" + msg.nickname + "']");
            foreach (XmlNode player in deletedPlayers)
            {
                player.ParentNode.RemoveChild(player);
            }
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayersRemoved(ClearPlayersMessageToServer msg)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNodeList playerCards = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerName='" + msg.nickname + "']/Hands/Hand/Cards/Card");
            foreach (XmlNode card in playerCards)
            {
                card.ParentNode.RemoveChild(card);
            }
            XmlNodeList playerChips = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerName='" + msg.nickname + "']/Hands/Hand/Chips/Chip");
            foreach (XmlNode chip in playerChips)
            {
                chip.ParentNode.RemoveChild(chip);
            }
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayerBet(BetMessageToServer msg)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode chips = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand/Chips");
            XmlElement chip = xmlDoc.CreateElement("Chip");
            chip.SetAttribute("ChipAmount", msg.betAmount.ToString());
            chips.AppendChild(chip);
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void DealCards(Deck deck)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNodeList players = xmlDoc.SelectNodes("/Table/Players/Player");
            for(int i = 0; i < 2; i++)
            {
                int count = 0;
                foreach (XmlNode player in players)
                {
                    count++;
                    XmlElement card = xmlDoc.CreateElement("Card");
                    card.SetAttribute("CardRank", deck[0].Rank);
                    card.SetAttribute("CardSuit", deck[0].Suit);
                    card.SetAttribute("CardValue", deck[0].CardValue.ToString());
                    player.FirstChild.FirstChild.FirstChild.AppendChild(card);
                    deck.RemoveAt(0);
                }
                XmlElement dealerCard = xmlDoc.CreateElement("Card");
                dealerCard.SetAttribute("CardRank", deck[0].Rank);
                dealerCard.SetAttribute("CardSuit", deck[0].Suit);
                dealerCard.SetAttribute("CardValue", deck[0].CardValue.ToString());
                XmlNode dealerCards = xmlDoc.SelectSingleNode("Table/Dealer/DealerCards");
                dealerCards.AppendChild(dealerCard);
                deck.RemoveAt(0);
            }
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayerHit(HitMessageToServer msg, Deck deck)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode hand = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='"+ msg.handId +"']");
            XmlElement card = xmlDoc.CreateElement("Card");
            card.SetAttribute("CardRank", deck[0].Rank);
            card.SetAttribute("CardSuit", deck[0].Suit);
            card.SetAttribute("CardValue", deck[0].CardValue.ToString());
            hand.FirstChild.AppendChild(card);
            deck.RemoveAt(0);
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void PlayerDouble(DoubleMessageToServer msg, Deck deck)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode hand = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']");
            XmlElement card = xmlDoc.CreateElement("Card");
            card.SetAttribute("CardRank", deck[0].Rank);
            card.SetAttribute("CardSuit", deck[0].Suit);
            card.SetAttribute("CardValue", deck[0].CardValue.ToString());
            hand.FirstChild.AppendChild(card);
            deck.RemoveAt(0);
            XmlNode chips = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']/Chips");
            XmlNodeList currentChips = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']/Chips/Chip");
            for (int i = 0; i < currentChips.Count; i++)
            {
                XmlElement newChip = xmlDoc.CreateElement("Chip");
                newChip.SetAttribute("ChipAmount", currentChips[i].Attributes["ChipAmount"].Value);
                chips.AppendChild(newChip);
            }
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }


        public void PlayerSplit(SplitMessageToServer msg, Deck deck)
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNode hands = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands");
            XmlElement newHand = xmlDoc.CreateElement("Hand");
            XmlElement newCards = xmlDoc.CreateElement("Cards");
            XmlElement newChips = xmlDoc.CreateElement("Chips");
            newHand.SetAttribute("HandId", (msg.handId + 1).ToString());
            
            XmlNode card = null;
            XmlNodeList cards = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']/Cards/Card");
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == 1)
                    card = cards[i];
            }
            newCards.AppendChild(card);

            XmlNode hand = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']");
            XmlElement firstCard = xmlDoc.CreateElement("Card");
            firstCard.SetAttribute("CardRank", deck[0].Rank);
            firstCard.SetAttribute("CardSuit", deck[0].Suit);
            firstCard.SetAttribute("CardValue", deck[0].CardValue.ToString());
            hand.FirstChild.AppendChild(firstCard);
            deck.RemoveAt(0);

            XmlElement secondCard = xmlDoc.CreateElement("Card");
            secondCard.SetAttribute("CardRank", deck[0].Rank);
            secondCard.SetAttribute("CardSuit", deck[0].Suit);
            secondCard.SetAttribute("CardValue", deck[0].CardValue.ToString());
            newCards.AppendChild(secondCard);
            deck.RemoveAt(0);

            newHand.AppendChild(newCards);

            XmlNode chips = xmlDoc.SelectSingleNode("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId+1 + "']/Chips");
            XmlNodeList currentChips = xmlDoc.SelectNodes("/Table/Players/Player[@PlayerId='" + msg.playerId + "']/Hands/Hand[@HandId='" + msg.handId + "']/Chips/Chip");
            for (int i = 0; i < currentChips.Count; i++)
            {
                XmlElement newChip = xmlDoc.CreateElement("Chip");
                newChip.SetAttribute("ChipAmount", currentChips[i].Attributes["ChipAmount"].Value);
                newChips.AppendChild(newChip);
            }
            newHand.AppendChild(newChips);

            hands.AppendChild(newHand);
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public void DealerRemove()
        {
            this.rwl.AcquireWriterLock(10000);
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNodeList dealerCards = xmlDoc.SelectNodes("/Table/Dealer/DealerCards/Card");
            foreach (XmlNode card in dealerCards)
            {
                card.ParentNode.RemoveChild(card);
            }
            xmlDoc.Save(filename);
            this.rwl.ReleaseWriterLock();
        }

        public XmlDocument GetXmlDoc()
        {
            //string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
            string filename = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "Table1.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            return xmlDoc;
        }
    }
}
