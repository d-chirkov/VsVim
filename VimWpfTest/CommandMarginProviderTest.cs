﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Moq;
using Vim.UI.Wpf.Implementation.CommandMargin;
using Vim.UnitTest;
using Xunit;

namespace Vim.UI.Wpf.UnitTest
{
    public sealed class CommandMarginProviderTest : VimTestBase
    {
        private readonly MockRepository _factory;
        private readonly Mock<IVim> _vim;
        private CommandMarginProvider _commandMarginProviderRaw;
        private IWpfTextViewMarginProvider _commandMarginProvider;

        public CommandMarginProviderTest()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _vim = _factory.Create<IVim>();
            _commandMarginProviderRaw = new CommandMarginProvider(
                _vim.Object,
                CompositionContainer.GetExportedValue<IEditorFormatMapService>(),
                new List<Lazy<IOptionsProviderFactory>>());
            _commandMarginProvider = _commandMarginProviderRaw;
        }

        /// <summary>
        /// If the host won't create an IVimBuffer then the command margin shouldn't force one
        /// to be created
        /// </summary>
        [Fact]
        public void HostWontCreateVimBuffer()
        {
            var textView = CreateTextView();
            var wpfTextViewHost = TextEditorFactoryService.CreateTextViewHost(textView, setFocus: false);
            _vim.Setup(x => x.GetOrCreateVimBufferForHost(textView)).Returns(FSharpOption<IVimBuffer>.None);
            Assert.Null(_commandMarginProvider.CreateMargin(wpfTextViewHost, _factory.Create<IWpfTextViewMargin>().Object));
        }
    }
}