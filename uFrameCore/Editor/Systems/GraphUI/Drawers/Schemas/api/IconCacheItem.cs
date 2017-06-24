using UnityEngine;

namespace uFrame.Editor.GraphUI.Drawers.Schemas
{
    internal struct IconCacheItem
    {
        public string Name { get; set; }
        public Color TintColor { get; set; }

        public bool Equals(IconCacheItem other) {
            return string.Equals(Name, other.Name) && TintColor.Equals(other.TintColor);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IconCacheItem && Equals((IconCacheItem) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ TintColor.GetHashCode();
            }
        }
    }
}