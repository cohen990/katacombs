﻿using System.Collections.Generic;
using System.Linq;
using Codurance_Katacombs.Commands;
using FakeItEasy;
using NUnit.Framework;

namespace Codurance_Katacombs.Acceptance
{
    [TestFixture]
    public class MovingAroundTheWorldFeature
    {
        private IWrapConsole _fakeConsole;
        private IKatacombsEngine _katacombsEngine;
        private KatacombsController _katacombsController;
        private List<string> _readMessages;

        [SetUp]
        public void TestSetup()
        {
            _readMessages = new List<string>();
            _fakeConsole = A.Fake<IWrapConsole>();
            
            _katacombsEngine = new KatacombsEngine(new KatacombsWorld(), new CommandFactory());
            _katacombsController = new KatacombsController(_katacombsEngine, _fakeConsole);

            _katacombsEngine.ShowMessage += (messageText) => _readMessages.AddRange(messageText);
        }

        [Test]
        public void Changing_location_always_trigger_next_location_message()
        {
            Given_I_startup_the_game_with_3_locations();
            When_I_move_to_all_locations_and_back();
            Then_all_location_messages_should_have_shown_in_correct_order();
        }

        private void Given_I_startup_the_game_with_3_locations()
        {
            _katacombsController.StartGame();
        }

        private void When_I_move_to_all_locations_and_back()
        {
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO N");
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO W");
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO UP");
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO DOWN");
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO E");
            _fakeConsole.ReadLine += Raise.FreeForm.With("GO S");
        }

        private void Then_all_location_messages_should_have_shown_in_correct_order()
        {
            Assert.That(_readMessages.Count, Is.EqualTo(14));
            Assert.That(_readMessages.ElementAt(0), Is.EqualTo(_readMessages.ElementAt(12)));
            Assert.That(_readMessages.ElementAt(1), Is.EqualTo(_readMessages.ElementAt(13)));
        }
    }
}
