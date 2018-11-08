using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheatGameApp
{
    public partial class CardLabel : Label
    {
        public event CancelEventHandler SelectedChanged;
        public static readonly Size CardSize = new Size(79, 123);
        private bool m_facingUp = true;
        private bool m_selected = false;
        private bool m_selectClick = false;
        private Color m_selectedColor = Color.FromArgb(60, Color.Blue);
        [DefaultValue(typeof(Card), "Ace of Clubs")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [RefreshProperties(RefreshProperties.All)]
        [Localizable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Card Card { get; set; }
        [DefaultValue(false)]
        public bool Selected
        {
            get { return m_selected; }
            set
            {
                if (m_selected == value)
                    return;
                bool oldValue = m_selected;
                m_selected = value;
                if (SelectedChanged != null)
                {
                    CancelEventArgs e = new CancelEventArgs();
                    SelectedChanged(this, e);
                    if (e.Cancel)
                        m_selected = oldValue;
                }

                Invalidate();
            }
        }
        [DefaultValue(true)]
        public bool FacingUp
        {
            get { return m_facingUp; }
            set { m_facingUp = value; Invalidate(); }
        }
        [DefaultValue(typeof(Color), "60,0,0,255")]
        public Color SelectedColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; Invalidate(); }
        }
        [DefaultValue(false)]
        public bool SelectClick
        {
            get { return m_selectClick; }
            set { m_selectClick = value; Cursor = m_selectClick ? Cursors.Hand : Cursors.Default; }
        }

        public CardLabel()
        {
            InitializeComponent();
            this.AutoSize = false;
            this.Text = string.Empty;
            //BackColor = Color.Transparent;
            BackColor = Color.Green;// SystemColors.Control;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Image cardsImg = global::CheatGameApp.Properties.Resources.cards;

            Size cardSize = CardSize;// new Size(cardsImg.Width / 13, cardsImg.Height / 5);

            Card card = m_facingUp ? this.Card : Card.EmptyDeck;
            int x = cardSize.Width * (card.Number - 1);
            int y = cardSize.Height * (int)card.Type;

            Rectangle source = new Rectangle(new Point(x, y), cardSize);

            e.Graphics.DrawImage(cardsImg, new Rectangle(Point.Empty, Size), source, GraphicsUnit.Pixel);
            if (Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(m_selectedColor), e.ClipRectangle);
            }
            base.OnPaint(e);
        }
        protected override void OnClick(EventArgs e)
        {
            if (SelectClick)
                this.Selected = !this.Selected;
            base.OnClick(e);
        }
    }
}
