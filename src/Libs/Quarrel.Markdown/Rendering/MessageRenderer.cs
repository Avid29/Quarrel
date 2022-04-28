﻿// Quarrel © 2022

using Quarrel.Markdown.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Quarrel.Markdown
{
    [TemplatePart(Name = nameof(RichBlockPartName), Type = typeof(RichTextBlock))]
    public sealed partial class MessageRenderer : Control
    {
        private const string RichBlockPartName = "RichBlock";

        private const bool IsTextSelectable = false;

        private RichTextBlock? _richBlock;
        private Canvas? _canvas;
        private Grid? _grid;

        private List<Inline> _codeBlocks = new List<Inline>();

        public MessageRenderer()
        {
            this.DefaultStyleKey = typeof(MessageRenderer);
        }

        protected override void OnApplyTemplate()
        {
            //_richBlock = (RichTextBlock)GetTemplateChild(RichBlockPartName);
            _canvas = (Canvas)GetTemplateChild("Canvas");
            _grid = (Grid)GetTemplateChild("Grid");
        }
        
        private int a;
        private void RichBlock_SizeChanged(object sender, object e)
        {
            UpdateOverlays();
        }

        private void UpdateOverlays()
        {
            _canvas.Children.Clear();
            foreach (var inline in _codeBlocks)
            {
                var startRect = inline.ElementStart.GetCharacterRect(inline.ElementStart.LogicalDirection);
                var endRect = inline.ElementEnd.GetCharacterRect(inline.ElementStart.LogicalDirection);
                int heightPadding = 1;
                var rec = new Rectangle()
                {
                    Width = Math.Max(endRect.Right - startRect.Left, 0),
                    Height = Math.Max(endRect.Bottom - startRect.Top + heightPadding*2, 0),
                    Fill = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                    RadiusX = 2,
                    RadiusY = 2
                };
                _canvas.Children.Add(rec);
                Canvas.SetTop(rec, startRect.Top - heightPadding);
                Canvas.SetLeft(rec, startRect.Left);
            }
        }

        private void RenderMarkdown(IList<ASTRoot> tree)
        {
            if (_richBlock != null)
            {
                _richBlock.SizeChanged -= RichBlock_SizeChanged;
                _grid.Children.Remove(_richBlock);
            }

            _richBlock = new RichTextBlock();
            _richBlock.SizeChanged += RichBlock_SizeChanged;
            BlockCollection blocks = _richBlock.Blocks;
            _codeBlocks.Clear();

            foreach (var root in tree)
            {
                var paragraph = new Paragraph();
                blocks.Add(paragraph);
                InlineCollection inlineCollection = paragraph.Inlines;
                Stack<(IAST, InlineCollection)> stack = new Stack<(IAST, InlineCollection)>();
                foreach (IAST ast in root.Children.Reverse())
                {
                    stack.Push((ast, inlineCollection));
                }

                while (stack.TryPop(out var tuple))
                {
                    (IAST node, inlineCollection) = tuple;
                    switch (node)
                    {
                        case CodeBlock codeBlock:
                            {
                                var container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new CodeBlockElement(codeBlock)
                                {
                                    FontSize = container.FontSize,
                                    FontWeight = container.FontWeight,
                                    FontStretch = container.FontStretch,
                                    TextDecorations = container.TextDecorations,
                                    Style = CodeBlockStyle,
                                };
                            }
                            break;
                        case BlockQuote blockQuote:
                            {
                                var container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                var element = new BlockQuoteElement(blockQuote)
                                {
                                    FontSize = container.FontSize,
                                    FontWeight = container.FontWeight,
                                    FontStretch = container.FontStretch,
                                    TextDecorations = container.TextDecorations,
                                    Style = BlockQuoteStyle,
                                };
                                container.Child = element;

                                foreach (IAST child in blockQuote.Children.Reverse())
                                {
                                    stack.Push((child, element.Paragraph.Inlines));
                                }
                            }
                            break;
                        case Url url:
                            inlineCollection.Add(new Hyperlink() { NavigateUri = new Uri(url.Content), Inlines = { new Run() { Text = url.Content } } });
                            break;
                        case IEmojiAST emoji:
                            {
                                var container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new EmojiElement(emoji)
                                {
                                    Style = EmojiStyle,
                                };
                            }
                            break;
                        case Strong strong:
                            {
                                var inline = new Bold();
                                inlineCollection.Add(inline);
                                foreach (IAST child in strong.Children.Reverse())
                                {
                                    stack.Push((child, inline.Inlines));
                                }
                            }
                            break;
                        case Em em:
                            {
                                var inline = new Italic();
                                inlineCollection.Add(inline);
                                foreach (IAST child in em.Children.Reverse())
                                {
                                    stack.Push((child, inline.Inlines));
                                }
                            }
                            break;
                        case U u:
                            {
                                var inline = new Underline();
                                inlineCollection.Add(inline);
                                foreach (IAST child in u.Children.Reverse())
                                {
                                    stack.Push((child, inline.Inlines));
                                }
                            }
                            break;
                        case S s:
                            {
                                var inline = new Span() { TextDecorations = TextDecorations.Strikethrough };
                                inlineCollection.Add(inline);
                                foreach (IAST child in s.Children.Reverse())
                                {
                                    stack.Push((child, inline.Inlines));
                                }
                            }
                            break;
                        case InlineCode inlineCode:
                            {
                                var inline = new Span();
                                inlineCollection.Add(inline);
                                inline.Inlines.Add(new Run() { Text = " " });
                                inline.Inlines.Add(new Run()
                                {
                                    FontFamily = new FontFamily("Consolas"),
                                    Text = inlineCode.Content,
                                });
                                inline.Inlines.Add(new Run() { Text = " " });
                                _codeBlocks.Add(inline);
                            }   
                            break;
                        case Timestamp timeStamp:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new TimestampElement(timeStamp)
                                {
                                    Style = TimestampStyle,
                                };
                            }
                            break;
                        case RoleMention roleMention:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new HyperlinkButton()
                                {
                                    RenderTransform = new TranslateTransform { Y = 4 },
                                    Background = new SolidColorBrush(Colors.DarkGray),
                                    Padding = new Thickness(0),
                                    Content = new TextBlock()
                                    {
                                        FontWeight = container.FontWeight,
                                        FontSize = container.FontSize,
                                        FontStretch = container.FontStretch,
                                        TextDecorations = container.TextDecorations,
                                        IsTextSelectionEnabled = IsTextSelectable,
                                        Text = roleMention.RoleID
                                    }
                                };
                            }
                            break;
                        case UserMention mention:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new UserMentionElement(mention, Context)
                                {
                                    Style = UserMentionStyle,
                                };
                            }
                            break;
                        case GlobalMention globalMention:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new HyperlinkButton()
                                {
                                    RenderTransform = new TranslateTransform { Y = 4 },
                                    Background = new SolidColorBrush(Colors.DarkGray),
                                    Padding = new Thickness(0),
                                    Content = new TextBlock()
                                    {
                                        FontWeight = container.FontWeight,
                                        FontSize = container.FontSize,
                                        FontStretch = container.FontStretch,
                                        TextDecorations = container.TextDecorations,
                                        IsTextSelectionEnabled = IsTextSelectable,
                                        Text = globalMention.Target,
                                    }
                                };
                            }
                            break;
                        case ChannelMention channel:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                container.Child = new HyperlinkButton()
                                {
                                    RenderTransform = new TranslateTransform { Y = 4 },
                                    Background = new SolidColorBrush(Colors.DarkGray),
                                    Padding = new Thickness(0),
                                    Content = new TextBlock()
                                    {
                                        FontWeight = container.FontWeight,
                                        FontSize = container.FontSize,
                                        FontStretch = container.FontStretch,
                                        TextDecorations = container.TextDecorations,
                                        IsTextSelectionEnabled = IsTextSelectable,
                                        Text = channel.ChannelID
                                    }
                                };
                            }
                            break;
                        case Spoiler spoiler:
                            {
                                InlineUIContainer container = new InlineUIContainer();
                                inlineCollection.Add(container);
                                var element = new SpoilerElement(spoiler)
                                {
                                    FontFamily = container.FontFamily,
                                    FontWeight = container.FontWeight,
                                    FontSize = container.FontSize,
                                    FontStretch = container.FontStretch,
                                    TextDecorations = container.TextDecorations,
                                    Style = SpoilerStyle,
                                };
                                container.Child = element;

                                foreach (IAST child in spoiler.Children.Reverse())
                                {
                                    stack.Push((child, element.Paragraph.Inlines));
                                }
                            }
                            break;
                        case Text textNode:
                            inlineCollection.Add(new Run() { Text = textNode.Content });
                            break;
                    }
                }
            }
            _grid.Children.Add(_richBlock);
        }

        /// <summary>
        /// Hacky function to force split code blocks and block quotes onto new lines
        /// </summary>
        private static IList<ASTRoot> AdjustTree(IList<IAST> tree)
        {
            // TODO: modify tree inpace
            List<ASTRoot> newTree = new List<ASTRoot>() { new ASTRoot(new List<IAST>()) };
            Stack<IAST> stack = new Stack<IAST>();
            foreach (IAST child in tree.Reverse())
            {
                stack.Push(child);
            }
            List<(int, ASTChildren)> depths = new List<(int, ASTChildren)>();
            depths.Add((stack.Count, newTree[0]));
            while (stack.TryPop(out IAST node))
            {
                int depth;
                ASTChildren current;
                do
                {
                    (depth, current) = depths[depths.Count - 1];
                    depths.RemoveAt(depths.Count - 1);
                } while (depth <= 0);
                depth--;
                depths.Add((depth, current));
                switch (node)
                {
                    case BlockQuote:
                    case CodeBlock:
                        {
                            (int, ASTChildren)[] newDepths = new (int, ASTChildren)[depths.Count];

                            ASTChildren currentCodeBlockNode = current with
                            {
                                Children = new List<IAST>() { node }
                            };

                            newDepths[newDepths.Length - 1] = (depth, current with { Children = new List<IAST>() });
                            for (int i = depths.Count - 2; i >= 0; i--)
                            {
                                (int oldDepth, ASTChildren oldNode) = depths[i];
                                newDepths[i] = (oldDepth,
                                    oldNode with { Children = new List<IAST>() { newDepths[i + 1].Item2 } });

                                currentCodeBlockNode = oldNode with
                                {
                                    Children = new List<IAST>() { currentCodeBlockNode }
                                };
                            }

                            newTree.Add((ASTRoot)currentCodeBlockNode);
                            depths = newDepths.ToList();
                            newTree.Add((ASTRoot)newDepths[0].Item2);
                        }
                        break;
                    case ASTChildren astChildrenNode:
                        {
                            foreach (IAST child in astChildrenNode.Children.Reverse())
                            {
                                stack.Push(child);
                            }

                            var tmp = astChildrenNode with { Children = new List<IAST>() };
                            current.Children.Add(tmp);
                            depths.Add((astChildrenNode.Children.Count, tmp));
                        }
                        break;
                    default:
                        current.Children.Add(node);
                        break;
                }
            }

            return newTree;
        }
    }
}
