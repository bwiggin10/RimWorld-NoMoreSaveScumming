using Verse;
using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace NoMoreSaveScumming
{
    [StaticConstructorOnStartup]
    public static class NoMoreSaveScumming
    {
        public static FieldInfo ticksSinceLastAutoSave = null;

        private static void ForceSave()
        {
            Current.Game.autosaver.DoAutosave();
            ticksSinceLastAutoSave.SetValue(Current.Game.autosaver, 0);
        }

        static NoMoreSaveScumming()
        {
            ticksSinceLastAutoSave = AccessTools.Field(typeof(Autosaver), "ticksSinceSave");
            if (ticksSinceLastAutoSave == null)
            {
                throw new Exception("Unable to retrieve ticksSinceSave");
            }
            Harmony harmony = new Harmony("NoMoreSaveScumming_Ben");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //Harmony.DEBUG = true;
        }

        [HarmonyPatch(typeof(LetterStack), nameof(LetterStack.ReceiveLetter), new Type[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(LookTargets), typeof(Faction), typeof(Quest), typeof(List<ThingDef>), typeof(string), typeof(int), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal })]
        public static class Patch_LetterStack_ReceiveLetter
        {
            public static void Postfix(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
            {
                if (Current.Game.Info.permadeathMode == true)
                {
                    List<LetterDef> letterDefsToForceSave = new List<LetterDef>
                    {
                        LetterDefOf.ThreatBig,
                        LetterDefOf.ThreatSmall,
                        LetterDefOf.NegativeEvent,
                        LetterDefOf.NeutralEvent,
                        LetterDefOf.PositiveEvent,
                        LetterDefOf.Death,
                        LetterDefOf.AcceptVisitors,
                        LetterDefOf.AcceptJoiner,
                        LetterDefOf.GameEnded,
                        LetterDefOf.ChoosePawn,
                        LetterDefOf.RitualOutcomeNegative,
                        LetterDefOf.RitualOutcomePositive,
                        LetterDefOf.RelicHuntInstallationFound,
                        LetterDefOf.BabyBirth,
                        LetterDefOf.ChildBirthday,
                        LetterDefOf.Bossgroup,
                        LetterDefOf.AcceptCreepJoiner,
                        LetterDefOf.EntityDiscovered
                };

                    if (letterDefsToForceSave.Contains(textLetterDef))
                    {
                        ForceSave();
                    }
                    else
                    {
                        Log.Error("Failed to determine the LetterDefOf category this event pertains to: " + textLetterDef.defName);
                    }
                }
                else
                {
                }
            }
        }




    }
}
