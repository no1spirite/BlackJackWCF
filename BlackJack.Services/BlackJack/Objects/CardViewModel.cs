
namespace BlackJack.Services.BlackJack.Objects
{
    public class CardViewModel
    {
        public string Rank { get; set; }
        public string Suit { get; set; }
        public string CardBackImage { get; set; }
        public string CardImage { get; set; }
        public int CardValue { get; set; }
        private int _isVisible;
        public int IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
            }
        }

        private int _cardOpacity;
        public int CardOpacity
        {
            get { return this._cardOpacity; }
            set { this._cardOpacity = value; }
        }

        private bool _cardDealt;
        public bool CardDealt
        {
            get { return this._cardDealt; }
            set
            {
                this._cardDealt = value;
            }
        }


        private int _cardXPos;
        public int CardXPos
        {
            get { return this._cardXPos; }
            set
            {
                this._cardXPos = value;
            }
        }

        public int CardYPos { get; set; }
        public int CardZPos { get; set; }

        private int _cardRotationY;
        public int CardRotationY
        {
            get { return this._cardRotationY; }
            set
            {
                this._cardRotationY = value;
            }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get
            {
                //if (!IsVisible)
                //    return CardBackImage;
                //else
                //    return CardImage;
                return this._currentImage;
            }
            set
            {
                this._currentImage = value;
            }
        }

        public enum CardSuit
        {
            Spade,
            Heart,
            Diamond,
            Club
        } ;

        public enum CardRank
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        } ;

        public enum FlipStatus
        {
            FaceUp,
            FaceDown,
            Flip
        } ;
    }
}