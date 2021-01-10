﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Athena.Data;
using NotImplementedException = System.NotImplementedException;

namespace Athena.Import {
    public class SeriesExtractor {
        public static Series Extract(string text) {
            var pattern = @"(?:(?:(?![a-z])(?: ?-? ?[Tt]om ))(?<volumeNumber>\d+))| - (\d+)\/\d+";
            if (text == "'-" || text == "-" || string.IsNullOrEmpty(text)) {
                return new Series {
                    SeriesName = null,
                };
            }

            var regex = new Regex(pattern);
            var matches = regex.Matches(text).ToList();
            int volumeNumber;
            string seriesName;
            if (matches.Count > 1) {
                throw new ExtractorException("Cannot extract data from text", text);
            }
            if (matches.Count == 0 ) {
                seriesName = text;
                volumeNumber = 0;
            }
            else {
                var match = matches[0];
                var matchText = match.Value;
                volumeNumber = ExtractVolumeNumber(match, text);
                seriesName = text.Replace(matchText, "");
            }

            return new Series {
                Id = Guid.NewGuid(),
                SeriesName = seriesName,
                VolumeNumber = volumeNumber
            };
        }

        private static int ExtractVolumeNumber(Match match, string text) {
            if (match.Groups[1].Length >= 1) {
                return Convert.ToInt32(match.Groups[1].Value);
            }
            else if (match.Groups[2].Length >= 1) {
                return Convert.ToInt32(match.Groups[2].Value);
            }
            else {
                throw new ExtractorException("Cannot extract data from text", text);
            }
        }
    }
}