﻿using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace RotaryEncoderSample
{
    internal sealed class DisplayService
    {
        private AbsoluteLayout _rootLayout;
        private IFont _font;
        private Picture _picture;
        private Label _label;
        private Timer _clearTimer;

        public DisplayScreen Screen { get; }

        public DisplayService(IColorInvertableDisplay display, ITouchScreen touchscreen)
        {
            display.InvertDisplayColor(true);

            Screen = new DisplayScreen(display, RotationType._270Degrees, touchscreen)
            {
                BackgroundColor = Color.FromRgb(0xEF, 0xF4, 0xF0)
            };

            BuildContent();

            _clearTimer = new Timer((_) =>
            {
                _label.Text = string.Empty;
            });
        }

        private void BuildContent()
        {
            _font = new Font16x24();

            _rootLayout = new AbsoluteLayout(Screen.Width, Screen.Height);

            _picture = new Picture(
                    left: (Screen.Width - 120) / 2,
                    top: 10,
                    width: 120,
                    height: 120
                )
            {
                Image = Image.LoadFromFile("space-monkey.bmp")
            };

            _label = new Label(
                left: 0,
                top: _picture.Bottom + 20,
                width: Screen.Width,
                height: 30
                )
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Font = _font,
                TextColor = Color.DimGray
            };

            _rootLayout.Controls.Add(_picture, _label);

            Screen.Controls.Add(_rootLayout);
        }

        public void SetLabelText(string text)
        {
            if (text != _label.Text)
            {
                _label.Text = text;
            }

            _clearTimer.Change(1000, Timeout.Infinite);
        }
    }
}