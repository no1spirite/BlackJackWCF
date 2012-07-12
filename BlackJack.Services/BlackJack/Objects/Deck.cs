namespace BlackJack.Services.BlackJack.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deck : List<CardViewModel>
    {
        private Object _lock = new object();
        public Deck()
        {
            this.CreateDeck(4);
            //StackDeck();
            this.ShuffleDeck();
        }

        public void StackDeck()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Ace.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Ace.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 11
                        });
                this.Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.King.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.King.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
                this.Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Queen.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Queen.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
                this.Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Jack.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Jack.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
            }
        }

        public void CreateDeck(int deckCount)
        {
            lock (this._lock)
            {
                this.Clear();
                for (int i = 0; i < deckCount; i++)
                {
                    List<object> cardSuits = EnumHelper.GetValues(typeof (CardViewModel.CardSuit)).ToList();
                    List<object> cardRanks = EnumHelper.GetValues(typeof (CardViewModel.CardRank)).ToList();
                    foreach (CardViewModel.CardSuit suit in cardSuits)
                        foreach (CardViewModel.CardRank rank in cardRanks)
                        {
                            switch (rank)
                            {
                                case CardViewModel.CardRank.Ace:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 11
                                            });
                                    break;
                                case CardViewModel.CardRank.Two:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 2
                                            });
                                    break;
                                case CardViewModel.CardRank.Three:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 3
                                            });
                                    break;
                                case CardViewModel.CardRank.Four:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 4
                                            });
                                    break;
                                case CardViewModel.CardRank.Five:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 5
                                            });
                                    break;
                                case CardViewModel.CardRank.Six:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 6
                                            });
                                    break;
                                case CardViewModel.CardRank.Seven:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 7
                                            });
                                    break;
                                case CardViewModel.CardRank.Eight:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 8
                                            });
                                    break;
                                case CardViewModel.CardRank.Nine:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 9
                                            });
                                    break;
                                case CardViewModel.CardRank.Jack:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                                case CardViewModel.CardRank.Queen:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                                case CardViewModel.CardRank.King:
                                    this.Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                            }

                        }
                }
            }
        }

        public void ShuffleDeck()
        {
            lock (this._lock)
            {
                var randomList = new List<CardViewModel>();
                var r = new Random();
                while (this.Count > 0)
                {
                    int randomIndex = r.Next(0, this.Count);
                    randomList.Add(this[randomIndex]);
                    this.RemoveAt(randomIndex);
                }
                this.Clear();
                while (randomList.Count > 0)
                {
                    int index = r.Next(0, randomList.Count);
                    this.Add(randomList[index]);
                    randomList.RemoveAt(index);
                }
            }
        }
    }
}