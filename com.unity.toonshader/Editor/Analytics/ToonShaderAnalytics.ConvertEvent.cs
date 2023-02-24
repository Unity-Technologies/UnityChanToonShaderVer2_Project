﻿namespace UnityEditor.Rendering.Toon {
internal partial class ToonShaderAnalytics {
    internal class ConvertEvent : AnimeToolboxAnalytics.Event<ConvertEvent.EventData> {
        internal override string eventName => EVENT_NAME_PREFIX + "convert";
        internal override int    maxItems  => 1;

        internal struct EventData {
            public string converter;
        }

        internal ConvertEvent(string converter)
            : base(new EventData { converter = converter }) {
        }
    }
}
}
