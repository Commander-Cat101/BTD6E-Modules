﻿global using Ultra_Powers.PowerAdapters;
global using Assets.Scripts.Models.Powers;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(Ultra_Powers.UltraPowers), "Ultra Powers", "1.6", "1330 Studios LLC")]

namespace Ultra_Powers;
internal sealed class UltraPowers : MelonMod {
    public static readonly List<IPowerAdapter> powerAdapters = new() {
        new UltraCashDrop(),
        new UltraThrive(),
        new UltraGlueTrap(),
        new UltraTechBot(),
        new UltraRoadSpikes(),
        new UltraDartTime(),
        new UltraMonkeyBoost()
    };

    public static GameModel gameModel;

    public override void OnApplicationStart() {
        MelonLogger.Msg("Ultra Powers loaded!");
        HarmonyInstance.Patch(Method(typeof(GameModelLoader), nameof(GameModelLoader.Load)), null, new(Method(GetType(), nameof(GameLoaded))));

        powerAdapters.ForEach(adapter => adapter.Setup(ref Assets.SpriteAssets, ref Assets.RendererAssets));
    }

    public static void GameLoaded(ref GameModel __result) {
        gameModel = __result;

        for (int i = 0; i < __result.powers.Length; i++) {
            var power = __result.powers[i];

            foreach (var adapter in powerAdapters)
                adapter.ModifyPower(ref power);

            __result.powers[i] = power;
        }
    }
}