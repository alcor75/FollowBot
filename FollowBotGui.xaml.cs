using System.Windows;
using System.Windows.Controls;
using FollowBot.Class;
using log4net;
using DreamPoeBot.Loki.Common;

namespace FollowBot
{
    /// <summary>
    /// Logica di interazione per LabRunnerBotGui.xaml
    /// </summary>
    public partial class FollowBotGui : UserControl
    {
		private static readonly ILog Log = Logger.GetLoggerInstanceForType();
		public FollowBotGui()
        {
            InitializeComponent();
        }

        private void RemoveDefensiveSkillRule(object sender, RoutedEventArgs e)
        {
            DefensiveSkillsClass rule = (sender as Button).DataContext as DefensiveSkillsClass;
            FollowBotSettings.Instance.DefensiveSkills.Remove(rule);
        }

		private void AddGlobalNameIgnoreButton_OnClick(object sender, RoutedEventArgs e)
		{
			string text = GlobalNameIgnoreTextBox.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}

			if (!FollowBotSettings.Instance.GlobalNameIgnoreList.Contains(text))
			{
				FollowBotSettings.Instance.GlobalNameIgnoreList.Add(text);
				FollowBotSettings.Instance.UpdateGlobalNameIgnoreList();
				GlobalNameIgnoreTextBox.Text = "";
			}
			else
			{
				Log.ErrorFormat(
					"[AddGlobalNameIgnoreButtonOnClick] The skillgem {0} is already in the GlobalNameIgnoreList.", text);
			}
		}

		private void RemoveGlobalNameIgnoreButton_OnClick(object sender, RoutedEventArgs e)
		{
			string text = GlobalNameIgnoreTextBox.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}

			if (FollowBotSettings.Instance.GlobalNameIgnoreList.Contains(text))
			{
				FollowBotSettings.Instance.GlobalNameIgnoreList.Remove(text);
				FollowBotSettings.Instance.UpdateGlobalNameIgnoreList();
				GlobalNameIgnoreTextBox.Text = "";
			}
			else
			{
				Log.ErrorFormat("[RemoveGlobalNameIgnoreButtonOnClick] The skillgem {0} is not in the GlobalNameIgnoreList.", text);
			}
		}
		private void GlobalNameIgnoreListListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e != null && e.AddedItems.Count > 0)
			{
				GlobalNameIgnoreTextBox.Text = e.AddedItems[0].ToString();
			}
		}
	}
}
