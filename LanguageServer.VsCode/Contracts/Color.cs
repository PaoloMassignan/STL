using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts
{

//    interface Color
//    {

//        /**
//         * The red component of this color in the range [0-1].
//         */
//        readonly red: number;

//	/**
//	 * The green component of this color in the range [0-1].
//	 */
//	readonly green: number;

//	/**
//	 * The blue component of this color in the range [0-1].
//	 */
//	readonly blue: number;

//	/**
//	 * The alpha component of this color in the range [0-1].
//	 */
//	readonly alpha: number;
//}

    public class Color
    {
        [JsonConstructor]
        public Color()
        {

        }
        public Color(int red,int green, int blue, int alpha)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        [JsonProperty]
        public int red { get; set; }
        [JsonProperty]
        public int green { get; set; }
        [JsonProperty]
        public int blue { get; set; }
        [JsonProperty]
        public int alpha { get; set; }
    }

//    interface ColorInformation
//    {
//        /**
//         * The range in the document where this color appears.
//         */
//        range: Range;

//	/**
//	 * The actual color value for this color range.
//	 */
//	color: Color;
//}

    public class ColorInformation
    {
        [JsonConstructor]
        public ColorInformation()
        {

        }

        [JsonProperty]
        public Range range { get; set; }
        [JsonProperty]
        public Color color { get; set; }
    }

}
