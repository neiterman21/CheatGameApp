using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheatGameApp.Model;

namespace CheatGameApp
{
    public partial class DeckLabel : UserControl
    {
        public enum LayoutStyle
        {
            Horizontal,
            Vertical,
            Sorted,
        }
        public event EventHandler<DeckEventArgs> SelectionChanged;
        private void RaiseSelectionChanged()
        {
            if (SelectionChanged == null || m_supressSelectionChanged)
                return;
            SelectionChanged(this, new DeckEventArgs(GetSelectedCards()));
        }
        private bool m_supressSelectionChanged = false;
        public void SupressSelectionChanged()
        {
            m_supressSelectionChanged = true;
        }
        public void ResumeSelectionChanged()
        {
            m_supressSelectionChanged = false;
        }

        private List<CardLabel> m_cardLabels = new List<CardLabel>();
        private bool m_selectClick = false;
        private Deck m_deck = new Deck();
        private Size m_cardSize = CardLabel.CardSize;
        private bool m_facingUp = true;
        private LayoutStyle m_style = LayoutStyle.Sorted;


        [DefaultValue(LayoutStyle.Sorted)]
        public LayoutStyle Style
        {
            get { return m_style; }
            set
            {
                if (m_style == value)
                    return;
                m_style = value;
                RepositionCards();
            }
        }

        [DefaultValue(4)]
        public int MaxSelection
        {
            get;
            set;
        }
        [DefaultValue(typeof(Size), "79,123")]
        public Size CardSize 
        {
            get { return m_cardSize; }
            set { m_cardSize = value; RepositionCards(); }
        }
        public string DeckCards
        {
            get { return m_deck.ToShortString(); }
            set { Deck = Deck.Parse(value); }
        }
        public Deck Deck
        {
            get { return m_deck; }
            set { m_deck = value; RecreateCards(); }
        }
        [DefaultValue(true)]
        public bool FacingUp
        {
            get { return m_facingUp; }
            set
            {
                if (m_facingUp == value)
                    return;

                m_facingUp = value;
                foreach (CardLabel cardLabel in m_cardLabels)
                {
                    cardLabel.FacingUp = m_facingUp;
                }
            }
        }
        [DefaultValue(false)]
        public bool SelectClick { get { return m_selectClick; } set { m_selectClick = value; RepositionCards(); } }
        public DeckLabel()
        {
            MaxSelection = 4;
            InitializeComponent();
            BackColor = Color.Transparent;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RepositionCards();
        }
        private void RepositionSorted()
        {
            var cardsByNumber = m_deck.ToDictionary();

            //get size
            Size cardSize = CardSize;
            if (cardsByNumber.Count * cardSize.Width > this.Width)
            {
                int width = this.Width / cardsByNumber.Count;
                double ratio = cardSize.Height / (double)cardSize.Width;
                int height = (int)Math.Round(width * ratio);
                cardSize = new Size(width, height);
            }

            int startX = (this.Width - cardsByNumber.Count * cardSize.Width) / 2;
            foreach (CardLabel cardLabel in m_cardLabels)
            {
                Point offset = (Point)cardLabel.Tag;
                cardLabel.Size = cardSize;
                cardLabel.SelectClick = m_selectClick;
                cardLabel.Left = offset.X * cardSize.Width + startX;
                cardLabel.Top = offset.Y * cardSize.Height / 5;
            }
        }
        private void RepositionHorizontal()
        {
            Size cardSize = this.CardSize;
            int width = Math.Min(cardSize.Width * m_cardLabels.Count, this.Width);
            int startX = (this.Width - width) / 2;

            int offset = (m_cardLabels.Count == 1) ? 0 : (width - cardSize.Width) / (m_cardLabels.Count - 1);
            for (int i = 0; i < m_cardLabels.Count; i++)
            {
                CardLabel cardLabel = m_cardLabels[i];
                cardLabel.Left = startX + offset * i;
                cardLabel.Top = 0;
                cardLabel.Size = cardSize;
            }
        }
        private void RepositionCards()
        {
            bool isVisible = this.Visible;
            this.SuspendLayout();
            if (isVisible)
                this.Visible = false;
            switch (m_style)
            {
                case LayoutStyle.Horizontal:
                    RepositionHorizontal();
                    break;
                case LayoutStyle.Vertical:
                    break;
                case LayoutStyle.Sorted:
                default:
                    RepositionSorted();
                    break;
            }
            this.ResumeLayout(true);
            if (isVisible)
                this.Visible = isVisible;
        }
        public void SelectAll()
        {
            foreach (CardLabel cardLabel in m_cardLabels)
            {
                cardLabel.Selected = true;
            }
        }
        
        public void SelectNone()
        {
            foreach (CardLabel cardLabel in m_cardLabels)
            {
                cardLabel.Selected = false;
            }
        }
        public void RecreateCards()
        {
            bool isVisible = this.Visible;
            if (isVisible)
                this.Visible = false;

            try
            {

                this.SuspendLayout();
                foreach (CardLabel cardLabel in m_cardLabels)
                    cardLabel.SelectedChanged -= OnCardLabel_SelectedChanged;

                //m_cardLabels.Clear();
                if (m_style != LayoutStyle.Horizontal)
                    m_deck.Sort();
                var cardsByNumber = m_deck.ToDictionary();

                if (m_cardLabels.Count > m_deck.Count)
                {
                    int over = m_cardLabels.Count - m_deck.Count;

                    for (int i = 0; i < over; i++)
                    {
                        m_cardLabels.RemoveAt(m_cardLabels.Count - 1);
                        this.Controls.RemoveAt(Controls.Count - 1);
                    }
                }

                int cardsCount = 0;
                //validation
                if (cardsByNumber.Count != 0)
                {
                    //foreach number
                    int xOffset = 0;
                    foreach (var cards in cardsByNumber)
                    {
                        //for each card
                        for (int i = cards.Value.Count - 1; i >= 0; i--)
                        {

                            CardLabel cardLabel;

                            if (cardsCount >= m_cardLabels.Count)
                            {
                                cardLabel = new CardLabel();
                                m_cardLabels.Add(cardLabel);
                                this.Controls.Add(cardLabel);

                            }
                            else
                            {
                                cardLabel = m_cardLabels[cardsCount];
                            }
                            this.Controls.SetChildIndex(cardLabel, cardsCount);
                            cardsCount++;

                            cardLabel.AutoSize = false;
                            cardLabel.Card = cards.Value[i];
                            cardLabel.SelectClick = this.SelectClick;
                            cardLabel.FacingUp = m_facingUp;
                            cardLabel.Tag = new Point(xOffset, i);

                            cardLabel.SelectedChanged += OnCardLabel_SelectedChanged;

                        }
                        xOffset++;
                    }
                    RepositionCards();
                }

                //
                //this.Controls.Clear();
                //this.Controls.AddRange(m_cardLabels.ToArray());
                //this.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (isVisible)
                    this.Visible = true;
            }
        }

        private void OnCardLabel_SelectedChanged(object sender, CancelEventArgs e)
        {
            if (GetSelectedCards().Count > MaxSelection)
                e.Cancel = true;
            else
                RaiseSelectionChanged();
        }

        public Deck GetSelectedCards()
        {
            Deck deck = new Deck();

            foreach (CardLabel cardLabel in this.Controls)
            {
                if (!cardLabel.Selected)
                    continue;
                deck.Add(cardLabel.Card);
            }

            return deck;
        }
    }
}
