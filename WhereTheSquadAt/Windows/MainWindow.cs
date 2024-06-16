using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using Dalamud.Game.ClientState.Objects.SubKinds;


namespace WhereTheSquadAt.Windows;

public class MainWindow : Window, IDisposable
{
    private IDalamudTextureWrap? GoatImage;
    private Plugin Plugin;

    private IPartyList PartyList;
    private IClientState Client;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, IDalamudTextureWrap? goatImage, IPartyList partyList, IClientState client)
        : base("My Amazing Window##With a hidden ID", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        GoatImage = goatImage;
        Plugin = plugin;
        PartyList = partyList;
        Client = client;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (ImGui.Begin("Party Locations"))
        {
            var playerPosition = Client.LocalPlayer?.Position ?? Vector3.Zero;
            foreach (var member in PartyList)
            {
                var player = member.GameObject as PlayerCharacter;
                if (player != null)
                {
                    var partyMemberPosition = player.Position;
                    var distance = Vector3.Distance(playerPosition, partyMemberPosition);
                    ImGui.Text($"{player.Name}: {player.CurrentWorld.GameData.Name} {distance}");
                }
            }
            ImGui.End();
        }
        ImGui.Text($"The randoms config bool is {Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings"))
        {
            Plugin.ToggleConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
        if (GoatImage != null)
        {
            ImGuiHelpers.ScaledIndent(55f);
            ImGui.Image(GoatImage.ImGuiHandle, new Vector2(GoatImage.Width, GoatImage.Height));
            ImGuiHelpers.ScaledIndent(-55f);
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }
}
