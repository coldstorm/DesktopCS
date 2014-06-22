using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesktopCS.Services;
using DesktopCS.Models;
using DesktopCS.Controls;

namespace DesktopCS.Tests.Services
{
    [TestClass]
    public class TabManagerTests
    {
        private TabManager tabManager;

        [TestInitialize]
        public void InitTabManager()
        {
            this.tabManager = new TabManager();
        }

        [TestMethod]
        public void AddTabTest()
        {
            this.tabManager.AddChannel("testChannel");

            Assert.IsTrue(this.tabManager.ChannelTabs.Count == 1);
        }

        [TestMethod]
        public void AddDuplicateTabTest()
        {
            this.tabManager.AddChannel("testChannel");
            this.tabManager.AddChannel("testChannel");

            Assert.IsTrue(this.tabManager.ChannelTabs.Count == 1);
        }

        [TestMethod]
        public void SelectTabTest()
        {
            ChannelTab tab = this.tabManager.AddChannel("testChannel");

            Assert.AreEqual(null, this.tabManager.SelectedTab);

            tab.IsSelected = true;

            Assert.AreEqual(tab, this.tabManager.SelectedTab);
        }

        [TestMethod]
        public void CloseTabTest()
        {
            ChannelTab tab = this.tabManager.AddChannel("testChannel");

            Assert.IsTrue(this.tabManager.ChannelTabs.Count == 1);

            tab.TabItem.RaiseEvent(new System.Windows.RoutedEventArgs(CSTabItem.CloseTabEvent));

            Assert.IsTrue(this.tabManager.ChannelTabs.Count == 0);
        }
    }
}
