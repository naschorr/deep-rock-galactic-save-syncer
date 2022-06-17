using DeepRockGalacticSaveSyncer.Enums;
using DeepRockGalacticSaveSyncer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSyncer.DwarfManager
{
    internal static class DwarfFactory
    {
        private static readonly byte[] ENGINEER_BYTES_SIGNATURE = new byte[] { 133, 239, 98, 108, 101, 241, 2, 74, 141, 254, 181, 208, 243, 144, 157, 46, 3, 0, 0, 0, 88, 80 };
        private static readonly byte[] SCOUT_BYTES_SIGNATURE = new byte[] { 48, 216, 234, 23, 216, 251, 186, 76, 149, 48, 109, 233, 101, 92, 47, 140, 3, 0, 0, 0, 88, 80 };
        private static readonly byte[] DRILLER_BYTES_SIGNATURE = new byte[] { 158, 221, 86, 241, 238, 188, 197, 72, 141, 91, 94, 91, 128, 182, 45, 180, 3, 0, 0, 0, 88, 80 };
        private static readonly byte[] GUNNER_BYTES_SIGNATURE = new byte[] { 174, 86, 225, 128, 254, 192, 196, 77, 150, 250, 41, 194, 131, 102, 185, 123, 3, 0, 0, 0, 88, 80 };
        private static readonly int EXPERIENCE_BYTES_OFFSET = 48;
        private static readonly int PROMOTIONS_BYTES_OFFSET = 156;

        public static Dictionary<DwarfName, Dwarf> CreateDwarvesFromSaveFile(SaveFile saveFile)
        {
            var data = File.ReadAllBytes(saveFile.Path);

            // Find the location of the dwarf's data block in the save data bytes
            var engineerPosition = FindSubArrayInArray<byte>(ENGINEER_BYTES_SIGNATURE, data);
            var scoutPosition = FindSubArrayInArray<byte>(SCOUT_BYTES_SIGNATURE, data);
            var drillerPosition = FindSubArrayInArray<byte>(DRILLER_BYTES_SIGNATURE, data);
            var gunnerPosition = FindSubArrayInArray<byte>(GUNNER_BYTES_SIGNATURE, data);

            if (
                engineerPosition < 0 ||
                scoutPosition < 0 ||
                drillerPosition < 0 ||
                gunnerPosition < 0
            )
            {
                throw new IOException("Unable to find all dwarves in save file.");
            }

            // Total experience in current promotion
            var engineerExperience = (int)getUint32FromBytesAtOffset(data, engineerPosition + EXPERIENCE_BYTES_OFFSET);
            var scoutExperience = (int)getUint32FromBytesAtOffset(data, scoutPosition + EXPERIENCE_BYTES_OFFSET);
            var drillerExperience = (int)getUint32FromBytesAtOffset(data, drillerPosition + EXPERIENCE_BYTES_OFFSET);
            var gunnerExperience = (int)getUint32FromBytesAtOffset(data, gunnerPosition + EXPERIENCE_BYTES_OFFSET);

            // Number of promotions per dwarf
            var engineerPromotions = (int)getUint32FromBytesAtOffset(data, engineerPosition + PROMOTIONS_BYTES_OFFSET);
            var scoutPromotions = (int)getUint32FromBytesAtOffset(data, scoutPosition + PROMOTIONS_BYTES_OFFSET);
            var drillerPromotions = (int)getUint32FromBytesAtOffset(data, drillerPosition + PROMOTIONS_BYTES_OFFSET);
            var gunnerPromotions = (int)getUint32FromBytesAtOffset(data, gunnerPosition + PROMOTIONS_BYTES_OFFSET);

            var dwarves = new Dictionary<DwarfName, Dwarf>();
            dwarves.Add(DwarfName.Engineer, new Dwarf(engineerPromotions, engineerExperience));
            dwarves.Add(DwarfName.Scout, new Dwarf(scoutPromotions, scoutExperience));
            dwarves.Add(DwarfName.Driller, new Dwarf(drillerPromotions, drillerExperience));
            dwarves.Add(DwarfName.Gunner, new Dwarf(gunnerPromotions, gunnerExperience));

            return dwarves;
        }

        private static int FindSubArrayInArray<T>(IEnumerable<T> needle, IEnumerable<T> haystack) where T : IComparable
        {
            int haystackPosition = 0;
            foreach (var haystackDatum in haystack.SkipLast(needle.Count()))
            {
                int needlePosition = 0;
                foreach (var needleDatum in needle)
                {
                    // Is the haystack element different from the needle element?
                    if (haystack.ElementAt(haystackPosition + needlePosition).CompareTo(needleDatum) != 0)
                    {
                        break;
                    }

                    needlePosition += 1;
                }

                // Have we reached the end of the needle successfully?
                if (needlePosition == needle.Count())
                {
                    return haystackPosition;
                }

                haystackPosition += 1;
            }

            return -1;
        }

        private static UInt32 getUint32FromBytesAtOffset(byte[] data, int offset)
        {
            // Get array of length 4, because 32 = 4 bytes of 8 bits each
            var slice = data.Skip(offset).Take(4).ToArray();

            return (uint)(slice[3] << 24 | slice[2] << 16 | slice[1] << 8 | slice[0]);
        }
    }
}
