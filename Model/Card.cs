using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace CheatGameApp
{
    [TypeConverter(typeof(CardTypeConverter))]
    public struct Card : IComparable<Card>
    {
        public static readonly Card EmptyDeck = new Card()
        {
            m_number = 3,
            m_type = CardType.Deck
        };
        public const int MaxNumber = 13;

        private CardType m_type;
        private int m_number;

        public CardType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }
        public int Number
        {
            get { return MathHelper.Clamp(m_number, 1, MaxNumber); }
            set { m_number = MathHelper.Clamp(value, 1, MaxNumber); }
        }
        [Browsable(false)]
        public string NumberString
        {
            get
            {
                switch (Number)
                {
                    case 11:
                        return "Jack";
                    case 12:
                        return "Queen";
                    case 13:
                        return "King";
                    case 1:
                        return "Ace";
                    default:
                        return m_number.ToString();
                }
            }
        }

        public string RecordString
        {
            get
            {
                switch (Number)
                {
                    case 11:
                        return "Jack";
                    case 12:
                        return "Queen";
                    case 13:
                        return "King";
                    case 1:
                        return "Ace";
                    case 2:
                        return "Two";
                    case 3:
                        return "Three";
                    case 4:
                        return "Four";
                    case 5:
                        return "Five";
                    case 6:
                        return "Six";
                    case 7:
                        return "Seven";
                    case 8:
                        return "Eight";
                    case 9:
                        return "Nine";
                    case 10:
                        return "Ten";
                    default:
                        return m_number.ToString();
                }
            }
        }

        public Card(int number, CardType type)
        {
            m_number = type == CardType.Deck ? 3 : number;
            m_type = type == CardType.Deck? type: CardType.Heart;
        }

        public Card(Card copy)
        {
            m_number = copy.m_number;
            m_type = copy.m_type;
        }

        public Card(int[] cardcount) {
            m_type = CardType.Heart;
            m_number = 0;
            for (int i = 0; i < 13; i++)
            {
                if (cardcount[i] != 0)
                {
                    m_number = i + 1;
                }
            }
        }
        public Card Increase()
        {
            return new Card(m_number % Card.MaxNumber + 1, Type);
        }
        public Card Decrease()
        {
            return new Card(m_number == 1 ? Card.MaxNumber : m_number - 1, Type);
        }

        public bool isNear(Card operand)
        {
            return operand == Increase() || operand == Decrease();
        }

        public override string ToString()
        {
            return string.Format("{0} of {1}", NumberString, Type);
        }

        public static Card FromLiteral(string s)
        {
            s = s.ToLower();
            Card card = new Card() { Type = CardType.Heart };
            int number = 0;
            switch (s)
            {
                case "king":
                    number = 13;
                    break;
                case "queen":
                    number = 12;
                    break;
                case "jack":
                    number = 11;
                    break;
                case "ace":
                    number = 1;
                    break;
                case "two":
                    number = 2;
                    break;
                case "three":
                    number = 3;
                    break;
                case "four":
                    number = 4;
                    break;
                case "five":
                    number = 5;
                    break;
                case "six":
                    number = 6;
                    break;
                case "seven":
                    number = 7;
                    break;
                case "eight":
                    number = 8;
                    break;
                case "nine":
                    number = 9;
                    break;
                case "ten":
                    number = 10;
                    break;
                default:
                    MessageBox.Show("Internal error - Convert to Card from Literal unsuccessful");
                    break;
            }
            card.Number = number;
            return card;
        }
 
          public static Card Parse(string s)
        {
            s = s.ToLower();
            Card card;
            if (s.Contains(" of "))
            {
                string[] values = s.Split(new string[] { " of " }, StringSplitOptions.RemoveEmptyEntries);
                CardType type = (CardType)Enum.Parse(typeof(CardType), values[1], true);

                int number = 0;
                switch (values[0])
                {
                    case "king":
                        number = 13;
                        break;
                    case "queen":
                        number = 12;
                        break;
                    case "jack":
                        number = 11;
                        break;
                    case "ace":
                        number = 1;
                        break;
                    default:
                        number = int.Parse(values[0]);
                        break;
                }
                card = new Card(number, type);
            }
            else
            {
                int number = int.Parse(s.Remove(s.Length - 1));
                char cType = s.ToLower()[s.Length - 1];
                CardType type = CardType.Clubs;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                {
                    if (cardType.ToString().ToLower()[0] == cType)
                        break;
                    type = cardType;
                }

                card = new Card(number, type);
            }
            return card;
        }

        public int CompareTo(Card other)
        {
            if(other.Number == this.Number)
                return decimal.Compare((int)this.Type, (int)other.Type);

            return decimal.Compare(Number, other.Number);
        }

        public static bool operator >(Card operand1, Card operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Card operand1, Card operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        public static bool operator ==(Card operand1, Card operand2)
        {
            return operand1.CompareTo(operand2) == 0;
        }

        public static bool operator !=(Card operand1, Card operand2)
        {
            return operand1.CompareTo(operand2) != 0;
        }

    }
    public class CardTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            return destinationType == typeof(Card);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(string) && value is Card)
            {
                Card card = (Card)value;
                return card.ToString();
            }
            else return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    return Card.Parse(value as string);
                }
                catch
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "'to type Card");
                }
            }
            else return base.ConvertFrom(context, culture, value);
        }
        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return new Card((int)propertyValues["Number"], (CardType)propertyValues["Type"]);
        }
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);
            return properties;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
