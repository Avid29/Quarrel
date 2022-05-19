﻿// Quarrel © 2022

namespace Quarrel.ViewModels.SubPages.UserSettings
{
    /// <summary>
    /// An interface for items in the user settings navigation menu.
    /// </summary>
    public interface IUserSettingsMenuItem
    {
        /// <summary>
        /// Gets the title of the menu item.
        /// </summary>
        string Title { get; }
    }
}
