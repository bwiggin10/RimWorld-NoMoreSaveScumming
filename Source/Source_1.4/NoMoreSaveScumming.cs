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

        [HarmonyPatch(typeof(LetterStack), nameof(LetterStack.ReceiveLetter), new Type[] { typeof(TaggedString), typeof(TaggedString), typeof(LetterDef), typeof(LookTargets), typeof(Faction), typeof(Quest), typeof(List<ThingDef>), typeof(string) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal })]
        public static class Patch_LetterStack_ReceiveLetter
        {
            public static void Postfix(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
            {
                if (Current.Game.Info.permadeathMode == true)
                {
                    if (textLetterDef == LetterDefOf.ThreatBig)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.ThreatSmall)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.NegativeEvent)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.NeutralEvent)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.PositiveEvent)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.Death)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.NewQuest)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.AcceptVisitors)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.BetrayVisitors)
                    {
                        ForceSave();
                    }
                    else if (textLetterDef == LetterDefOf.ChoosePawn)
                    {
                        ForceSave();
                    }
                    else
                    {
                        Log.Error("Failed to detrmine the LetterDefOf category this event pertains to:" + textLetterDef.defName);
                    }
                }
                else
                {
                }
            }
        }




    }
}
