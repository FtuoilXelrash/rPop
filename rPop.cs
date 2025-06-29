using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("rPop", "Ftuoil Xelrash", "0.0.12")]
    [Description("Displays server population statistics and sends performance updates to Discord")]

    public class rPop : RustPlugin
    {
        #region Configuration

        private ConfigData config;
        private PluginData pluginData;
        private DateTime lastPopCommandTime = DateTime.MinValue;
        private Timer performanceTimer;
        private readonly Dictionary<string, DateTime> lastDiscordMessage = new Dictionary<string, DateTime>();

        public class ConfigData
        {
            [JsonProperty("Settings")] public PluginSettings Settings = new PluginSettings();
        }

        public class PluginSettings
        {
            [JsonProperty("Enable !pop Command")] public bool EnablePopCommand = true;
            [JsonProperty("Command Cooldown (minutes)")] public float CommandCooldown = 5f;
            [JsonProperty("Show Last Wipe Date")] public bool ShowLastWipeDate = true;
            
            [JsonProperty("Show Players Joining")] public bool ShowPlayersJoining = true;
            [JsonProperty("Show Players Sleeping")] public bool ShowPlayersSleeping = true;
            [JsonProperty("Show Admins Online")] public bool ShowAdminsOnline = true;
            [JsonProperty("Hide Zero Values")] public bool HideZeroValues = true;
            
            [JsonProperty("Discord Webhook URL")] public string WebhookURL = "";
            [JsonProperty("Discord Rate Limit (seconds)")] public float DiscordRateLimit = 1f;
            [JsonProperty("Enable Discord Performance Messages")] public bool EnablePerformanceMessages = true;
            [JsonProperty("Performance Message Interval (minutes)")] public float PerformanceMessageInterval = 60f;
            [JsonProperty("Enable In-Game Performance Messages")] public bool EnableInGamePerformanceMessages = true;
            
            [JsonProperty("Use Server Header Image")] public bool UseServerHeaderImage = true;
            [JsonProperty("Fallback Discord Image URL")] public string FallbackImageURL = "https://files.facepunch.com/lewis/1b2911b1/rust-logo.png";
            [JsonProperty("Use Thumbnail Instead of Image")] public bool UseThumbnail = true;
            
            [JsonProperty("Show Population Records")] public bool ShowPopulationRecords = true;
        }

        public class PluginData
        {
            [JsonProperty("Today High Population")] public PopulationRecord TodayHigh = new PopulationRecord();
            [JsonProperty("All Time High Population")] public PopulationRecord AllTimeHigh = new PopulationRecord();
            [JsonProperty("Last Reset Date")] public DateTime LastResetDate = DateTime.Today;
        }

        public class PopulationRecord
        {
            [JsonProperty("Count")] public int Count = 0;
            [JsonProperty("Date")] public DateTime Date = DateTime.MinValue;
        }

        protected override void LoadDefaultConfig()
        {
            config = new ConfigData();
            SaveConfig();
            Puts("Default configuration created.");
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<ConfigData>();
                if (config == null) 
                {
                    LoadDefaultConfig();
                    return;
                }
                ValidateConfiguration();
            }
            catch (Exception ex)
            {
                PrintError($"Error loading configuration: {ex.Message}");
                LoadDefaultConfig();
            }
        }

        private void ValidateConfiguration()
        {
            bool needsSave = false;

            // Only fix values that are clearly invalid, don't reset user preferences
            if (config.Settings.PerformanceMessageInterval < 5f)
            {
                Puts($"Performance message interval ({config.Settings.PerformanceMessageInterval}) is too low, setting to minimum of 5 minutes.");
                config.Settings.PerformanceMessageInterval = 5f;
                needsSave = true;
            }

            if (config.Settings.DiscordRateLimit < 0.1f)
            {
                Puts($"Discord rate limit ({config.Settings.DiscordRateLimit}) is too low, setting to minimum of 0.1 seconds.");
                config.Settings.DiscordRateLimit = 0.1f;
                needsSave = true;
            }

            // Check if we need to add missing properties without overwriting existing ones
            if (!ConfigHasAllProperties())
            {
                Puts("Adding missing configuration properties...");
                needsSave = true;
            }

            if (needsSave)
            {
                SaveConfig();
                Puts("Configuration updated with missing properties (existing settings preserved).");
            }
        }

        private bool ConfigHasAllProperties()
        {
            try
            {
                // Read the raw config file and check if it contains all required properties
                var configText = Config.ReadObject<Dictionary<string, object>>();
                var settings = configText.GetValueOrDefault("Settings") as Dictionary<string, object>;
                
                if (settings == null) return false;

                // Check for all properties including new ones
                bool hasAll = settings.ContainsKey("Discord Webhook URL") &&
                       settings.ContainsKey("Discord Rate Limit (seconds)") &&
                       settings.ContainsKey("Enable Discord Performance Messages") &&
                       settings.ContainsKey("Performance Message Interval (minutes)") &&
                       settings.ContainsKey("Enable In-Game Performance Messages") &&
                       settings.ContainsKey("Use Server Header Image") &&
                       settings.ContainsKey("Fallback Discord Image URL") &&
                       settings.ContainsKey("Use Thumbnail Instead of Image") &&
                       settings.ContainsKey("Show Players Joining") &&
                       settings.ContainsKey("Show Players Sleeping") &&
                       settings.ContainsKey("Show Admins Online") &&
                       settings.ContainsKey("Hide Zero Values") &&
                       settings.ContainsKey("Show Population Records");

                if (!hasAll)
                {
                    // Add missing properties with defaults, but preserve existing values
                    if (!settings.ContainsKey("Discord Webhook URL"))
                        config.Settings.WebhookURL = "";
                    if (!settings.ContainsKey("Discord Rate Limit (seconds)"))
                        config.Settings.DiscordRateLimit = 1f;
                    if (!settings.ContainsKey("Enable Discord Performance Messages"))
                        config.Settings.EnablePerformanceMessages = true;
                    if (!settings.ContainsKey("Performance Message Interval (minutes)"))
                        config.Settings.PerformanceMessageInterval = 60f;
                    if (!settings.ContainsKey("Enable In-Game Performance Messages"))
                        config.Settings.EnableInGamePerformanceMessages = true;
                    if (!settings.ContainsKey("Use Server Header Image"))
                        config.Settings.UseServerHeaderImage = true;
                    if (!settings.ContainsKey("Fallback Discord Image URL"))
                        config.Settings.FallbackImageURL = "https://files.facepunch.com/lewis/1b2911b1/rust-logo.png";
                    if (!settings.ContainsKey("Use Thumbnail Instead of Image"))
                        config.Settings.UseThumbnail = true;
                    if (!settings.ContainsKey("Show Players Joining"))
                        config.Settings.ShowPlayersJoining = true;
                    if (!settings.ContainsKey("Show Players Sleeping"))
                        config.Settings.ShowPlayersSleeping = true;
                    if (!settings.ContainsKey("Show Admins Online"))
                        config.Settings.ShowAdminsOnline = true;
                    if (!settings.ContainsKey("Hide Zero Values"))
                        config.Settings.HideZeroValues = true;
                    if (!settings.ContainsKey("Show Population Records"))
                        config.Settings.ShowPopulationRecords = true;
                }

                return hasAll;
            }
            catch
            {
                return false;
            }
        }

        protected override void SaveConfig()
        {
            try
            {
                Config.WriteObject(config, true);
            }
            catch (Exception ex)
            {
                PrintError($"Error saving configuration: {ex.Message}");
            }
        }

        #endregion

        #region Data Management

        private void LoadData()
        {
            try
            {
                pluginData = Interface.Oxide.DataFileSystem.ReadObject<PluginData>("rPop");
                if (pluginData == null)
                {
                    pluginData = new PluginData();
                    SaveData();
                }
                
                // Reset daily stats if it's a new day
                if (pluginData.LastResetDate.Date < DateTime.Today)
                {
                    pluginData.TodayHigh = new PopulationRecord();
                    pluginData.LastResetDate = DateTime.Today;
                    SaveData();
                    Puts("Daily population stats reset for new day.");
                }
            }
            catch (Exception ex)
            {
                PrintError($"Error loading data: {ex.Message}");
                pluginData = new PluginData();
                SaveData();
            }
        }

        private void SaveData()
        {
            try
            {
                Interface.Oxide.DataFileSystem.WriteObject("rPop", pluginData);
            }
            catch (Exception ex)
            {
                PrintError($"Error saving data: {ex.Message}");
            }
        }

        private void UpdatePopulationRecords()
        {
            try
            {
                int currentPopulation = BasePlayer.activePlayerList.Count;
                DateTime now = DateTime.Now;
                
                // Update today's high
                if (currentPopulation > pluginData.TodayHigh.Count)
                {
                    pluginData.TodayHigh.Count = currentPopulation;
                    pluginData.TodayHigh.Date = now;
                }
                
                // Update all-time high
                if (currentPopulation > pluginData.AllTimeHigh.Count)
                {
                    pluginData.AllTimeHigh.Count = currentPopulation;
                    pluginData.AllTimeHigh.Date = now;
                }
                
                SaveData();
            }
            catch (Exception ex)
            {
                PrintError($"Error updating population records: {ex.Message}");
            }
        }

        #endregion

        #region Hooks

        private void OnServerInitialized()
        {
            try
            {
                LoadData();
                
                if (config.Settings.EnablePerformanceMessages)
                    performanceTimer = timer.Every(config.Settings.PerformanceMessageInterval * 60f, SendPerformanceMessage);
                
                // Update population records on server start
                UpdatePopulationRecords();
            }
            catch (Exception ex)
            {
                PrintError($"Error during initialization: {ex.Message}");
            }
        }

        private void Unload()
        {
            performanceTimer?.Destroy();
            SaveData();
        }

        private void OnPlayerConnected(BasePlayer player)
        {
            // Update population records when a player connects
            timer.Once(1f, () => UpdatePopulationRecords());
        }

        private void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            // Update population records when a player disconnects (with slight delay)
            timer.Once(2f, () => UpdatePopulationRecords());
        }

        private void OnPlayerChat(BasePlayer player, string message, ConVar.Chat.ChatChannel channel)
        {
            if (player == null || string.IsNullOrEmpty(message)) return;

            if (message.ToLower() == "!pop")
                HandlePopCommand(player);
        }

        #endregion

        #region Pop Command

        private void HandlePopCommand(BasePlayer player)
        {
            if (!config.Settings.EnablePopCommand)
            {
                player.ChatMessage("The !pop command is currently disabled.");
                return;
            }

            var now = DateTime.Now;
            var timeSinceLastUse = now - lastPopCommandTime;

            if (timeSinceLastUse.TotalMinutes < config.Settings.CommandCooldown)
            {
                var remainingTime = TimeSpan.FromMinutes(config.Settings.CommandCooldown) - timeSinceLastUse;
                player.ChatMessage($"Server statistics are on cooldown. Try again in {GetCooldownTime(remainingTime)}.");
                return;
            }

            lastPopCommandTime = now;

            int playerCount = BasePlayer.activePlayerList.Count;
            int sleepingPlayers = BasePlayer.sleepingPlayerList.Count;
            int joiningPlayers = ServerMgr.Instance.connectionQueue.Queued;
            int maxPlayers = ConVar.Server.maxplayers;
            int adminCount = BasePlayer.activePlayerList.Count(p => p.IsAdmin);
            string uptime = GetHumanReadableUptime();

            string statsMessage = $"<color=#FFD700><size=14>LIVE SERVER STATISTICS</size></color>\n" +
                                $"<color=#00FF00>Players Online:</color> <color=#FFFFFF>{playerCount}/{maxPlayers}</color>";

            if (config.Settings.ShowPlayersJoining && (!config.Settings.HideZeroValues || joiningPlayers > 0))
                statsMessage += $"\n<color=#FFFF00>Players Joining:</color> <color=#FFFFFF>{joiningPlayers}</color>";

            if (config.Settings.ShowPlayersSleeping && (!config.Settings.HideZeroValues || sleepingPlayers > 0))
                statsMessage += $"\n<color=#FF0000>Players Sleeping:</color> <color=#FFFFFF>{sleepingPlayers}</color>";

            if (config.Settings.ShowAdminsOnline && (!config.Settings.HideZeroValues || adminCount > 0))
                statsMessage += $"\n<color=#FFB6C1>Admins Online:</color> <color=#FFFFFF>{adminCount}</color>";

            statsMessage += $"\n<color=#20B2AA>Server Online For:</color> <color=#FFFFFF>{uptime}</color>";

            if (config.Settings.ShowLastWipeDate)
            {
                string lastWipeDate = GetLastWipeDate();
                statsMessage += $"\n<color=#87CEEB>Last Wipe Date:</color> <color=#FFFFFF>{lastWipeDate}</color>";
            }

            foreach (var onlinePlayer in BasePlayer.activePlayerList)
                onlinePlayer?.ChatMessage(statsMessage);
        }

        #endregion

        #region Discord Performance

        private void SendPerformanceMessage()
        {
            try
            {
                int playerCount = BasePlayer.activePlayerList.Count;
                int sleepingPlayers = BasePlayer.sleepingPlayerList.Count;
                int joiningPlayers = ServerMgr.Instance.connectionQueue.Queued;
                int maxPlayers = ConVar.Server.maxplayers;
                int adminCount = BasePlayer.activePlayerList.Count(p => p.IsAdmin);
                float fps = Performance.current.frameRate;
                long memoryUsed = GC.GetTotalMemory(false) / 1024 / 1024;
                long totalMemory = SystemInfo.systemMemorySize;
                int mapEntities = BaseNetworkable.serverEntities.Count;
                string uptime = GetHumanReadableUptime();
                string lastWipeDate = GetLastWipeDate();

                string message = $"🟢 **Players Online:** `{playerCount}/{maxPlayers}`";

                if (config.Settings.ShowPlayersJoining && (!config.Settings.HideZeroValues || joiningPlayers > 0))
                    message += $"\n🟡 **Players Joining:** `{joiningPlayers}`";

                if (config.Settings.ShowPlayersSleeping && (!config.Settings.HideZeroValues || sleepingPlayers > 0))
                    message += $"\n🔴 **Players Sleeping:** `{sleepingPlayers}`";

                if (config.Settings.ShowAdminsOnline && (!config.Settings.HideZeroValues || adminCount > 0))
                    message += $"\n👑 **Admins Online:** `{adminCount}`";

                message += $"\n🏗️ **Map Entities:** `{mapEntities:N0}`\n" +
                          $"💾 **Memory Usage:** `{memoryUsed:N0} MB / {totalMemory:N0} MB`\n" +
                          $"⚡ **Server FPS:** `{fps:F1}`";

                if (config.Settings.ShowPopulationRecords)
                {
                    if (pluginData.TodayHigh.Count > 0)
                        message += $"\n📈 **Today's Peak Players:** `{pluginData.TodayHigh.Count}`";
                    
                    if (pluginData.AllTimeHigh.Count > 0)
                        message += $"\n🏆 **All-Time Peak Players:** `{pluginData.AllTimeHigh.Count}`";
                }

                message += $"\n🕐 **Server Online For:** `{uptime}`\n" +
                          $"🗺️ **Last Wipe Date:** `{lastWipeDate}`";

                SendDiscordMessage("📊 Server Performance", message, 65535);

                if (config.Settings.EnableInGamePerformanceMessages)
                    SendInGamePerformanceMessage(playerCount, sleepingPlayers, joiningPlayers, maxPlayers, adminCount, uptime, lastWipeDate);
            }
            catch (Exception ex)
            {
                PrintError($"Error sending performance message: {ex.Message}");
            }
        }

        private void SendInGamePerformanceMessage(int playerCount, int sleepingPlayers, int joiningPlayers, int maxPlayers, int adminCount, string uptime, string lastWipeDate)
        {
            try
            {
                string performanceMessage = $"<color=#FFD700><size=14>LIVE SERVER STATISTICS</size></color>\n" +
                                          $"<color=#00FF00>Players Online:</color> <color=#FFFFFF>{playerCount}/{maxPlayers}</color>";

                if (config.Settings.ShowPlayersJoining && (!config.Settings.HideZeroValues || joiningPlayers > 0))
                    performanceMessage += $"\n<color=#FFFF00>Players Joining:</color> <color=#FFFFFF>{joiningPlayers}</color>";

                if (config.Settings.ShowPlayersSleeping && (!config.Settings.HideZeroValues || sleepingPlayers > 0))
                    performanceMessage += $"\n<color=#FF0000>Players Sleeping:</color> <color=#FFFFFF>{sleepingPlayers}</color>";

                if (config.Settings.ShowAdminsOnline && (!config.Settings.HideZeroValues || adminCount > 0))
                    performanceMessage += $"\n<color=#FFB6C1>Admins Online:</color> <color=#FFFFFF>{adminCount}</color>";

                performanceMessage += $"\n<color=#20B2AA>Server Online For:</color> <color=#FFFFFF>{uptime}</color>\n" +
                                     $"<color=#87CEEB>Last Wipe Date:</color> <color=#FFFFFF>{lastWipeDate}</color>";

                foreach (var player in BasePlayer.activePlayerList)
                    player?.ChatMessage(performanceMessage);
            }
            catch (Exception ex)
            {
                PrintError($"Error sending in-game performance message: {ex.Message}");
            }
        }

        #endregion

        #region Discord Integration

        private bool IsDiscordRateLimited(string messageType)
        {
            if (lastDiscordMessage.TryGetValue(messageType, out DateTime lastTime))
            {
                if ((DateTime.Now - lastTime).TotalSeconds < config.Settings.DiscordRateLimit)
                    return true;
            }
            lastDiscordMessage[messageType] = DateTime.Now;
            return false;
        }

        private string GetServerImageUrl()
        {
            try
            {
                if (config.Settings.UseServerHeaderImage)
                {
                    string headerImage = ConVar.Server.headerimage;
                    if (!string.IsNullOrEmpty(headerImage) && IsValidImageUrl(headerImage))
                        return headerImage;
                }
                return config.Settings.FallbackImageURL;
            }
            catch
            {
                return config.Settings.FallbackImageURL;
            }
        }

        private bool IsValidImageUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;
            
            try
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out Uri result)) return false;
                if (result.Scheme != "http" && result.Scheme != "https") return false;
                
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                string path = result.AbsolutePath.ToLower();
                
                return imageExtensions.Any(ext => path.EndsWith(ext)) || 
                       path.Contains("image") || 
                       result.Host.Contains("imgur") || 
                       result.Host.Contains("steam") ||
                       result.Host.Contains("facepunch");
            }
            catch
            {
                return false;
            }
        }

        private void SendDiscordMessage(string title, string description, int color)
        {
            try
            {
                string webhookUrl = config.Settings.WebhookURL;
                if (string.IsNullOrEmpty(webhookUrl)) return;

                string serverName = ConVar.Server.hostname ?? "Unknown Server";
                string serverImageUrl = GetServerImageUrl();

                string displayServerName = serverName.Length > 55 
                    ? serverName.Substring(0, 52) + "..." 
                    : serverName;

                string embedTitle = $"[{displayServerName}]\n{title}";

                var embed = new
                {
                    title = embedTitle,
                    description = description,
                    color = color,
                    thumbnail = config.Settings.UseThumbnail ? new { url = serverImageUrl } : null,
                    image = !config.Settings.UseThumbnail ? new { url = serverImageUrl } : null,
                    footer = new { text = "rPop Server Statistics" },
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                var payload = JsonConvert.SerializeObject(new
                {
                    username = "rPop",
                    avatar_url = "https://cdn-icons-png.flaticon.com/512/1161/1161388.png",
                    embeds = new[] { embed }
                });

                var headers = new Dictionary<string, string>
                {
                    ["Content-Type"] = "application/json",
                    ["User-Agent"] = "rPop/1.0"
                };

                webrequest.Enqueue(webhookUrl, payload, (code, response) =>
                {
                    if (code != 200 && code != 204)
                    {
                        PrintError($"Discord message failed: {title} (HTTP {code})");
                    }
                }, this, Core.Libraries.RequestMethod.POST, headers);
            }
            catch (Exception ex)
            {
                PrintError($"Error sending Discord message: {ex.Message}");
            }
        }

        #endregion

        #region Utilities

        private string GetHumanReadableUptime()
        {
            TimeSpan uptime = TimeSpan.FromSeconds(UnityEngine.Time.realtimeSinceStartup);
            
            if (uptime.TotalDays >= 1)
                return $"{(int)uptime.TotalDays} days, {uptime.Hours} hours, {uptime.Minutes} minutes";
            else if (uptime.TotalHours >= 1)
                return $"{uptime.Hours} hours, {uptime.Minutes} minutes";
            else if (uptime.TotalMinutes >= 1)
                return $"{uptime.Minutes} minutes, {uptime.Seconds} seconds";
            else
                return $"{uptime.Seconds} seconds";
        }

        private string GetLastWipeDate()
        {
            try
            {
                DateTime wipeTime = GetWipeTimeFromSaveFile();
                
                if (wipeTime == DateTime.MinValue)
                {
                    // Fallback to server startup time if no save file available
                    TimeSpan uptime = TimeSpan.FromSeconds(UnityEngine.Time.realtimeSinceStartup);
                    DateTime serverStart = DateTime.Now - uptime;
                    return $"{serverStart:MMM dd, yyyy} (Server Start)";
                }
                
                TimeSpan timeSinceWipe = DateTime.Now - wipeTime;
                
                if (timeSinceWipe.TotalDays < 1)
                {
                    return $"{wipeTime:MMM dd, yyyy} ({(int)timeSinceWipe.TotalHours}h ago)";
                }
                else if (timeSinceWipe.TotalDays < 7)
                {
                    return $"{wipeTime:MMM dd, yyyy} ({(int)timeSinceWipe.TotalDays}d ago)";
                }
                else
                {
                    return $"{wipeTime:MMM dd, yyyy} ({(int)timeSinceWipe.TotalDays}d ago)";
                }
            }
            catch (Exception ex)
            {
                PrintError($"Error getting last wipe date: {ex.Message}");
                return "Unknown";
            }
        }

        private DateTime GetWipeTimeFromSaveFile()
        {
            try
            {
                string serverIdentity = ConVar.Server.identity ?? "server";
                string serverPath = Path.Combine("server", serverIdentity);
                
                if (!Directory.Exists(serverPath)) 
                    return DateTime.MinValue;

                // Look for procedural map save files
                var saveFiles = Directory.GetFiles(serverPath, "ProceduralMap.*.sav", SearchOption.TopDirectoryOnly);
                
                if (saveFiles.Length == 0)
                    return DateTime.MinValue;

                // Get the most recent save file and use its creation time
                var latestSaveFile = saveFiles.OrderByDescending(f => File.GetCreationTime(f)).First();
                DateTime creationTime = File.GetCreationTime(latestSaveFile);
                
                return creationTime;
            }
            catch (Exception ex)
            {
                PrintError($"Error getting wipe time from save file: {ex.Message}");
                return DateTime.MinValue;
            }
        }

        private string GetCooldownTime(TimeSpan remainingTime)
        {
            if (remainingTime.TotalMinutes >= 1)
            {
                int minutes = (int)remainingTime.TotalMinutes;
                int seconds = remainingTime.Seconds;
                return seconds > 0 ? $"{minutes} minute(s) and {seconds} second(s)" : $"{minutes} minute(s)";
            }
            else
            {
                int seconds = Math.Max(1, (int)Math.Ceiling(remainingTime.TotalSeconds));
                return $"{seconds} second(s)";
            }
        }

        #endregion

        #region Console Commands

        [ConsoleCommand("rpop.reload")]
        private void RPopReloadCommand(ConsoleSystem.Arg arg)
        {
            LoadConfig();
            LoadData();
            
            performanceTimer?.Destroy();
            if (config.Settings.EnablePerformanceMessages)
                performanceTimer = timer.Every(config.Settings.PerformanceMessageInterval * 60f, SendPerformanceMessage);
            
            Puts("rPop configuration and data reloaded!");
            SendDiscordMessage("🔄 Config Reloaded", "rPop configuration reloaded via console command", 3447003);
        }

        [ConsoleCommand("rpop.test")]
        private void RPopTestCommand(ConsoleSystem.Arg arg)
        {
            int playerCount = BasePlayer.activePlayerList.Count;
            int sleepingPlayers = BasePlayer.sleepingPlayerList.Count;
            int joiningPlayers = ServerMgr.Instance.connectionQueue.Queued;
            int maxPlayers = ConVar.Server.maxplayers;
            int adminCount = BasePlayer.activePlayerList.Count(p => p.IsAdmin);
            string uptime = GetHumanReadableUptime();
            string lastWipeDate = GetLastWipeDate();

            Puts($"=== rPop Test Statistics ===");
            Puts($"Players Online: {playerCount}/{maxPlayers}");
            Puts($"Players Joining: {joiningPlayers}");
            Puts($"Players Sleeping: {sleepingPlayers}");
            Puts($"Admins Online: {adminCount}");
            Puts($"Server Uptime: {uptime}");
            Puts($"Last Wipe Date: {lastWipeDate}");
            Puts($"Today's Peak: {pluginData.TodayHigh.Count} players");
            Puts($"All-Time Peak: {pluginData.AllTimeHigh.Count} players");
            
            SendDiscordMessage("🧪 Test Message", 
                $"Test message sent via console command.\n" +
                $"**Server Time:** `{DateTime.Now:yyyy-MM-dd HH:mm:ss}`\n" +
                $"**Plugin Version:** `{Version}`", 
                16776960);
        }

        [ConsoleCommand("rpop.performance")]
        private void RPopPerformanceCommand(ConsoleSystem.Arg arg)
        {
            SendPerformanceMessage();
            Puts("Performance message sent to Discord!");
        }

        [ConsoleCommand("rpop.resetdata")]
        private void RPopResetDataCommand(ConsoleSystem.Arg arg)
        {
            pluginData = new PluginData();
            SaveData();
            Puts("rPop data has been reset!");
            SendDiscordMessage("🗑️ Data Reset", "Population records have been reset via console command", 16711680);
        }

        [ConsoleCommand("rpop.help")]
        private void RPopHelpCommand(ConsoleSystem.Arg arg)
        {
            Puts("rPop Console Commands:");
            Puts("rpop.test - Show current server statistics and send test Discord message");
            Puts("rpop.performance - Force send performance stats to Discord");
            Puts("rpop.reload - Reload configuration and data");
            Puts("rpop.resetdata - Reset all population records data");
            Puts("rpop.forceconfig - Force regenerate config file with all Discord settings");
            Puts("rpop.help - Show this help message");
            Puts("");
            Puts("Player Commands:");
            Puts("!pop - Display server statistics to all online players");
            Puts("");
            Puts("Configuration:");
            Puts("Data is stored in: oxide/data/rPop.json");
            Puts("Config is stored in: oxide/config/rPop.json");
            Puts("If Discord settings are missing from your config, use 'rpop.forceconfig'");
        }

        [ConsoleCommand("rpop.forceconfig")]
        private void RPopForceConfigCommand(ConsoleSystem.Arg arg)
        {
            LoadDefaultConfig();
            Puts("Configuration file regenerated with all default settings!");
        }

        #endregion
    }
}