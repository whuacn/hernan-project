// Type: Persits.PDF.BigNumber
// Assembly: Persits.PDF, Version=2.5.0.19989, Culture=neutral, PublicKeyToken=b8d8c63f1ff142a8
// Assembly location: C:\Users\hernan\Desktop\asppdf_net\Samples\Bin\Persits.PDF.dll

using System.Text;

namespace Persits.PDF
{
  internal class BigNumber
  {
    private string m_bstrNumber;
    private bool m_bZero;
    private byte[] m_arrBytes;
    private ushort m_nFCS;
    private static int[] m_arrTable1;
    private static int[] m_arrTable2;
    private static int[][] m_arrTable4;

    static BigNumber()
    {
      BigNumber.m_arrTable1 = new int[1287]
      {
        31,
        7936,
        47,
        7808,
        55,
        7552,
        59,
        7040,
        61,
        6016,
        62,
        3968,
        79,
        7744,
        87,
        7488,
        91,
        6976,
        93,
        5952,
        94,
        3904,
        103,
        7360,
        107,
        6848,
        109,
        5824,
        110,
        3776,
        115,
        6592,
        117,
        5568,
        118,
        3520,
        121,
        5056,
        122,
        3008,
        124,
        1984,
        143,
        7712,
        151,
        7456,
        155,
        6944,
        157,
        5920,
        158,
        3872,
        167,
        7328,
        171,
        6816,
        173,
        5792,
        174,
        3744,
        179,
        6560,
        181,
        5536,
        182,
        3488,
        185,
        5024,
        186,
        2976,
        188,
        1952,
        199,
        7264,
        203,
        6752,
        205,
        5728,
        206,
        3680,
        211,
        6496,
        213,
        5472,
        214,
        3424,
        217,
        4960,
        218,
        2912,
        220,
        1888,
        227,
        6368,
        229,
        5344,
        230,
        3296,
        233,
        4832,
        234,
        2784,
        236,
        1760,
        241,
        4576,
        242,
        2528,
        244,
        1504,
        248,
        992,
        271,
        7696,
        279,
        7440,
        283,
        6928,
        285,
        5904,
        286,
        3856,
        295,
        7312,
        299,
        6800,
        301,
        5776,
        302,
        3728,
        307,
        6544,
        309,
        5520,
        310,
        3472,
        313,
        5008,
        314,
        2960,
        316,
        1936,
        327,
        7248,
        331,
        6736,
        333,
        5712,
        334,
        3664,
        339,
        6480,
        341,
        5456,
        342,
        3408,
        345,
        4944,
        346,
        2896,
        348,
        1872,
        355,
        6352,
        357,
        5328,
        358,
        3280,
        361,
        4816,
        362,
        2768,
        364,
        1744,
        369,
        4560,
        370,
        2512,
        372,
        1488,
        376,
        976,
        391,
        7216,
        395,
        6704,
        397,
        5680,
        398,
        3632,
        403,
        6448,
        405,
        5424,
        406,
        3376,
        409,
        4912,
        410,
        2864,
        412,
        1840,
        419,
        6320,
        421,
        5296,
        422,
        3248,
        425,
        4784,
        426,
        2736,
        428,
        1712,
        433,
        4528,
        434,
        2480,
        436,
        1456,
        440,
        944,
        451,
        6256,
        453,
        5232,
        454,
        3184,
        457,
        4720,
        458,
        2672,
        460,
        1648,
        465,
        4464,
        466,
        2416,
        468,
        1392,
        472,
        880,
        481,
        4336,
        482,
        2288,
        484,
        1264,
        488,
        752,
        527,
        7688,
        535,
        7432,
        539,
        6920,
        541,
        5896,
        542,
        3848,
        551,
        7304,
        555,
        6792,
        557,
        5768,
        558,
        3720,
        563,
        6536,
        565,
        5512,
        566,
        3464,
        569,
        5000,
        570,
        2952,
        572,
        1928,
        583,
        7240,
        587,
        6728,
        589,
        5704,
        590,
        3656,
        595,
        6472,
        597,
        5448,
        598,
        3400,
        601,
        4936,
        602,
        2888,
        604,
        1864,
        611,
        6344,
        613,
        5320,
        614,
        3272,
        617,
        4808,
        618,
        2760,
        620,
        1736,
        625,
        4552,
        626,
        2504,
        628,
        1480,
        632,
        968,
        647,
        7208,
        651,
        6696,
        653,
        5672,
        654,
        3624,
        659,
        6440,
        661,
        5416,
        662,
        3368,
        665,
        4904,
        666,
        2856,
        668,
        1832,
        675,
        6312,
        677,
        5288,
        678,
        3240,
        681,
        4776,
        682,
        2728,
        684,
        1704,
        689,
        4520,
        690,
        2472,
        692,
        1448,
        696,
        936,
        707,
        6248,
        709,
        5224,
        710,
        3176,
        713,
        4712,
        714,
        2664,
        716,
        1640,
        721,
        4456,
        722,
        2408,
        724,
        1384,
        728,
        872,
        737,
        4328,
        738,
        2280,
        740,
        1256,
        775,
        7192,
        779,
        6680,
        781,
        5656,
        782,
        3608,
        787,
        6424,
        789,
        5400,
        790,
        3352,
        793,
        4888,
        794,
        2840,
        796,
        1816,
        803,
        6296,
        805,
        5272,
        806,
        3224,
        809,
        4760,
        810,
        2712,
        812,
        1688,
        817,
        4504,
        818,
        2456,
        820,
        1432,
        824,
        920,
        835,
        6232,
        837,
        5208,
        838,
        3160,
        841,
        4696,
        842,
        2648,
        844,
        1624,
        849,
        4440,
        850,
        2392,
        852,
        1368,
        865,
        4312,
        866,
        2264,
        868,
        1240,
        899,
        6200,
        901,
        5176,
        902,
        3128,
        905,
        4664,
        906,
        2616,
        908,
        1592,
        913,
        4408,
        914,
        2360,
        916,
        1336,
        929,
        4280,
        930,
        2232,
        932,
        1208,
        961,
        4216,
        962,
        2168,
        964,
        1144,
        1039,
        7684,
        1047,
        7428,
        1051,
        6916,
        1053,
        5892,
        1054,
        3844,
        1063,
        7300,
        1067,
        6788,
        1069,
        5764,
        1070,
        3716,
        1075,
        6532,
        1077,
        5508,
        1078,
        3460,
        1081,
        4996,
        1082,
        2948,
        1084,
        1924,
        1095,
        7236,
        1099,
        6724,
        1101,
        5700,
        1102,
        3652,
        1107,
        6468,
        1109,
        5444,
        1110,
        3396,
        1113,
        4932,
        1114,
        2884,
        1116,
        1860,
        1123,
        6340,
        1125,
        5316,
        1126,
        3268,
        1129,
        4804,
        1130,
        2756,
        1132,
        1732,
        1137,
        4548,
        1138,
        2500,
        1140,
        1476,
        1159,
        7204,
        1163,
        6692,
        1165,
        5668,
        1166,
        3620,
        1171,
        6436,
        1173,
        5412,
        1174,
        3364,
        1177,
        4900,
        1178,
        2852,
        1180,
        1828,
        1187,
        6308,
        1189,
        5284,
        1190,
        3236,
        1193,
        4772,
        1194,
        2724,
        1196,
        1700,
        1201,
        4516,
        1202,
        2468,
        1204,
        1444,
        1219,
        6244,
        1221,
        5220,
        1222,
        3172,
        1225,
        4708,
        1226,
        2660,
        1228,
        1636,
        1233,
        4452,
        1234,
        2404,
        1236,
        1380,
        1249,
        4324,
        1250,
        2276,
        1287,
        7188,
        1291,
        6676,
        1293,
        5652,
        1294,
        3604,
        1299,
        6420,
        1301,
        5396,
        1302,
        3348,
        1305,
        4884,
        1306,
        2836,
        1308,
        1812,
        1315,
        6292,
        1317,
        5268,
        1318,
        3220,
        1321,
        4756,
        1322,
        2708,
        1324,
        1684,
        1329,
        4500,
        1330,
        2452,
        1332,
        1428,
        1347,
        6228,
        1349,
        5204,
        1350,
        3156,
        1353,
        4692,
        1354,
        2644,
        1356,
        1620,
        1361,
        4436,
        1362,
        2388,
        1377,
        4308,
        1378,
        2260,
        1411,
        6196,
        1413,
        5172,
        1414,
        3124,
        1417,
        4660,
        1418,
        2612,
        1420,
        1588,
        1425,
        4404,
        1426,
        2356,
        1441,
        4276,
        1442,
        2228,
        1473,
        4212,
        1474,
        2164,
        1543,
        7180,
        1547,
        6668,
        1549,
        5644,
        1550,
        3596,
        1555,
        6412,
        1557,
        5388,
        1558,
        3340,
        1561,
        4876,
        1562,
        2828,
        1564,
        1804,
        1571,
        6284,
        1573,
        5260,
        1574,
        3212,
        1577,
        4748,
        1578,
        2700,
        1580,
        1676,
        1585,
        4492,
        1586,
        2444,
        1603,
        6220,
        1605,
        5196,
        1606,
        3148,
        1609,
        4684,
        1610,
        2636,
        1617,
        4428,
        1618,
        2380,
        1633,
        4300,
        1634,
        2252,
        1667,
        6188,
        1669,
        5164,
        1670,
        3116,
        1673,
        4652,
        1674,
        2604,
        1681,
        4396,
        1682,
        2348,
        1697,
        4268,
        1698,
        2220,
        1729,
        4204,
        1730,
        2156,
        1795,
        6172,
        1797,
        5148,
        1798,
        3100,
        1801,
        4636,
        1802,
        2588,
        1809,
        4380,
        1810,
        2332,
        1825,
        4252,
        1826,
        2204,
        1857,
        4188,
        1858,
        2140,
        1921,
        4156,
        1922,
        2108,
        2063,
        7682,
        2071,
        7426,
        2075,
        6914,
        2077,
        5890,
        2078,
        3842,
        2087,
        7298,
        2091,
        6786,
        2093,
        5762,
        2094,
        3714,
        2099,
        6530,
        2101,
        5506,
        2102,
        3458,
        2105,
        4994,
        2106,
        2946,
        2119,
        7234,
        2123,
        6722,
        2125,
        5698,
        2126,
        3650,
        2131,
        6466,
        2133,
        5442,
        2134,
        3394,
        2137,
        4930,
        2138,
        2882,
        2147,
        6338,
        2149,
        5314,
        2150,
        3266,
        2153,
        4802,
        2154,
        2754,
        2161,
        4546,
        2162,
        2498,
        2183,
        7202,
        2187,
        6690,
        2189,
        5666,
        2190,
        3618,
        2195,
        6434,
        2197,
        5410,
        2198,
        3362,
        2201,
        4898,
        2202,
        2850,
        2211,
        6306,
        2213,
        5282,
        2214,
        3234,
        2217,
        4770,
        2218,
        2722,
        2225,
        4514,
        2226,
        2466,
        2243,
        6242,
        2245,
        5218,
        2246,
        3170,
        2249,
        4706,
        2250,
        2658,
        2257,
        4450,
        2258,
        2402,
        2273,
        4322,
        2311,
        7186,
        2315,
        6674,
        2317,
        5650,
        2318,
        3602,
        2323,
        6418,
        2325,
        5394,
        2326,
        3346,
        2329,
        4882,
        2330,
        2834,
        2339,
        6290,
        2341,
        5266,
        2342,
        3218,
        2345,
        4754,
        2346,
        2706,
        2353,
        4498,
        2354,
        2450,
        2371,
        6226,
        2373,
        5202,
        2374,
        3154,
        2377,
        4690,
        2378,
        2642,
        2385,
        4434,
        2401,
        4306,
        2435,
        6194,
        2437,
        5170,
        2438,
        3122,
        2441,
        4658,
        2442,
        2610,
        2449,
        4402,
        2465,
        4274,
        2497,
        4210,
        2567,
        7178,
        2571,
        6666,
        2573,
        5642,
        2574,
        3594,
        2579,
        6410,
        2581,
        5386,
        2582,
        3338,
        2585,
        4874,
        2586,
        2826,
        2595,
        6282,
        2597,
        5258,
        2598,
        3210,
        2601,
        4746,
        2602,
        2698,
        2609,
        4490,
        2627,
        6218,
        2629,
        5194,
        2630,
        3146,
        2633,
        4682,
        2641,
        4426,
        2657,
        4298,
        2691,
        6186,
        2693,
        5162,
        2694,
        3114,
        2697,
        4650,
        2705,
        4394,
        2721,
        4266,
        2753,
        4202,
        2819,
        6170,
        2821,
        5146,
        2822,
        3098,
        2825,
        4634,
        2833,
        4378,
        2849,
        4250,
        2881,
        4186,
        2945,
        4154,
        3079,
        7174,
        3083,
        6662,
        3085,
        5638,
        3086,
        3590,
        3091,
        6406,
        3093,
        5382,
        3094,
        3334,
        3097,
        4870,
        3107,
        6278,
        3109,
        5254,
        3110,
        3206,
        3113,
        4742,
        3121,
        4486,
        3139,
        6214,
        3141,
        5190,
        3145,
        4678,
        3153,
        4422,
        3169,
        4294,
        3203,
        6182,
        3205,
        5158,
        3209,
        4646,
        3217,
        4390,
        3233,
        4262,
        3265,
        4198,
        3331,
        6166,
        3333,
        5142,
        3337,
        4630,
        3345,
        4374,
        3361,
        4246,
        3393,
        4182,
        3457,
        4150,
        3587,
        6158,
        3589,
        5134,
        3593,
        4622,
        3601,
        4366,
        3617,
        4238,
        3649,
        4174,
        3713,
        4142,
        3841,
        4126,
        4111,
        7681,
        4119,
        7425,
        4123,
        6913,
        4125,
        5889,
        4135,
        7297,
        4139,
        6785,
        4141,
        5761,
        4147,
        6529,
        4149,
        5505,
        4153,
        4993,
        4167,
        7233,
        4171,
        6721,
        4173,
        5697,
        4179,
        6465,
        4181,
        5441,
        4185,
        4929,
        4195,
        6337,
        4197,
        5313,
        4201,
        4801,
        4209,
        4545,
        4231,
        7201,
        4235,
        6689,
        4237,
        5665,
        4243,
        6433,
        4245,
        5409,
        4249,
        4897,
        4259,
        6305,
        4261,
        5281,
        4265,
        4769,
        4273,
        4513,
        4291,
        6241,
        4293,
        5217,
        4297,
        4705,
        4305,
        4449,
        4359,
        7185,
        4363,
        6673,
        4365,
        5649,
        4371,
        6417,
        4373,
        5393,
        4377,
        4881,
        4387,
        6289,
        4389,
        5265,
        4393,
        4753,
        4401,
        4497,
        4419,
        6225,
        4421,
        5201,
        4425,
        4689,
        4483,
        6193,
        4485,
        5169,
        4489,
        4657,
        4615,
        7177,
        4619,
        6665,
        4621,
        5641,
        4627,
        6409,
        4629,
        5385,
        4633,
        4873,
        4643,
        6281,
        4645,
        5257,
        4649,
        4745,
        4675,
        6217,
        4677,
        5193,
        4739,
        6185,
        4741,
        5161,
        4867,
        6169,
        4869,
        5145,
        5127,
        7173,
        5131,
        6661,
        5133,
        5637,
        5139,
        6405,
        5141,
        5381,
        5155,
        6277,
        5157,
        5253,
        5187,
        6213,
        5251,
        6181,
        5379,
        6165,
        5635,
        6157,
        6151,
        7171,
        6155,
        6659,
        6163,
        6403,
        6179,
        6275,
        6211,
        5189,
        4681,
        4433,
        4321,
        3142,
        2634,
        2386,
        2274,
        1612,
        1364,
        1252,
        856,
        744,
        496
      };
      BigNumber.m_arrTable2 = new int[78]
      {
        3,
        6144,
        5,
        5120,
        6,
        3072,
        9,
        4608,
        10,
        2560,
        12,
        1536,
        17,
        4352,
        18,
        2304,
        20,
        1280,
        24,
        768,
        33,
        4224,
        34,
        2176,
        36,
        1152,
        40,
        640,
        48,
        384,
        65,
        4160,
        66,
        2112,
        68,
        1088,
        72,
        576,
        80,
        320,
        96,
        192,
        129,
        4128,
        130,
        2080,
        132,
        1056,
        136,
        544,
        144,
        288,
        257,
        4112,
        258,
        2064,
        260,
        1040,
        264,
        528,
        513,
        4104,
        514,
        2056,
        516,
        1032,
        1025,
        4100,
        1026,
        2052,
        2049,
        4098,
        4097,
        2050,
        1028,
        520,
        272,
        160
      };
      BigNumber.m_arrTable4 = new int[65][]
      {
        new int[4]
        {
          72,
          2,
          69,
          3
        },
        new int[4]
        {
          66,
          10,
          65,
          0
        },
        new int[4]
        {
          74,
          12,
          67,
          8
        },
        new int[4]
        {
          70,
          5,
          71,
          11
        },
        new int[4]
        {
          73,
          9,
          68,
          1
        },
        new int[4]
        {
          65,
          1,
          70,
          12
        },
        new int[4]
        {
          67,
          5,
          66,
          8
        },
        new int[4]
        {
          69,
          4,
          74,
          11
        },
        new int[4]
        {
          71,
          3,
          73,
          10
        },
        new int[4]
        {
          68,
          9,
          72,
          6
        },
        new int[4]
        {
          70,
          11,
          66,
          4
        },
        new int[4]
        {
          73,
          5,
          67,
          12
        },
        new int[4]
        {
          74,
          10,
          65,
          2
        },
        new int[4]
        {
          72,
          1,
          71,
          7
        },
        new int[4]
        {
          68,
          6,
          69,
          9
        },
        new int[4]
        {
          65,
          3,
          73,
          6
        },
        new int[4]
        {
          71,
          4,
          67,
          7
        },
        new int[4]
        {
          66,
          1,
          74,
          9
        },
        new int[4]
        {
          72,
          10,
          70,
          2
        },
        new int[4]
        {
          69,
          0,
          68,
          8
        },
        new int[4]
        {
          71,
          2,
          65,
          4
        },
        new int[4]
        {
          73,
          11,
          66,
          0
        },
        new int[4]
        {
          74,
          8,
          68,
          12
        },
        new int[4]
        {
          67,
          6,
          72,
          7
        },
        new int[4]
        {
          70,
          1,
          69,
          10
        },
        new int[4]
        {
          66,
          12,
          71,
          9
        },
        new int[4]
        {
          72,
          3,
          73,
          0
        },
        new int[4]
        {
          70,
          8,
          74,
          7
        },
        new int[4]
        {
          69,
          6,
          67,
          10
        },
        new int[4]
        {
          68,
          4,
          65,
          5
        },
        new int[4]
        {
          73,
          4,
          70,
          7
        },
        new int[4]
        {
          72,
          11,
          66,
          9
        },
        new int[4]
        {
          71,
          0,
          74,
          6
        },
        new int[4]
        {
          65,
          6,
          69,
          8
        },
        new int[4]
        {
          67,
          1,
          68,
          2
        },
        new int[4]
        {
          70,
          9,
          73,
          12
        },
        new int[4]
        {
          69,
          11,
          71,
          1
        },
        new int[4]
        {
          74,
          5,
          72,
          4
        },
        new int[4]
        {
          68,
          3,
          66,
          2
        },
        new int[4]
        {
          65,
          7,
          67,
          0
        },
        new int[4]
        {
          66,
          3,
          69,
          1
        },
        new int[4]
        {
          71,
          10,
          68,
          5
        },
        new int[4]
        {
          73,
          7,
          74,
          4
        },
        new int[4]
        {
          67,
          11,
          70,
          6
        },
        new int[4]
        {
          65,
          8,
          72,
          12
        },
        new int[4]
        {
          69,
          2,
          73,
          1
        },
        new int[4]
        {
          70,
          10,
          68,
          0
        },
        new int[4]
        {
          74,
          3,
          65,
          9
        },
        new int[4]
        {
          71,
          5,
          67,
          4
        },
        new int[4]
        {
          72,
          8,
          66,
          7
        },
        new int[4]
        {
          70,
          0,
          69,
          5
        },
        new int[4]
        {
          67,
          3,
          65,
          10
        },
        new int[4]
        {
          71,
          12,
          74,
          2
        },
        new int[4]
        {
          68,
          11,
          66,
          6
        },
        new int[4]
        {
          73,
          8,
          72,
          9
        },
        new int[4]
        {
          70,
          4,
          65,
          11
        },
        new int[4]
        {
          66,
          5,
          67,
          2
        },
        new int[4]
        {
          74,
          1,
          69,
          12
        },
        new int[4]
        {
          73,
          3,
          71,
          6
        },
        new int[4]
        {
          72,
          0,
          68,
          7
        },
        new int[4]
        {
          69,
          7,
          72,
          5
        },
        new int[4]
        {
          65,
          12,
          66,
          11
        },
        new int[4]
        {
          67,
          9,
          74,
          0
        },
        new int[4]
        {
          71,
          8,
          70,
          3
        },
        new int[4]
        {
          68,
          10,
          73,
          2
        }
      };
    }

    public BigNumber()
    {
      this.m_arrBytes = new byte[14];
      //base.\u002Ector();
      this.m_bstrNumber = "";
      this.m_bZero = true;
      this.CheckForZeros();
    }

    public BigNumber(BigNumber num)
    {
      this.m_arrBytes = new byte[14];
      //base.\u002Ector();
      this.m_bstrNumber = num.m_bstrNumber;
      this.CheckForZeros();
    }

    public BigNumber(string strNum)
    {
      this.m_arrBytes = new byte[14];
      //base.\u002Ector();
      this.m_bstrNumber = strNum;
      this.CheckForZeros();
    }

    public BigNumber(int nNumber)
    {
      this.m_arrBytes = new byte[14];
      //base.\u002Ector();
      while (nNumber > 0)
      {
        this.m_bstrNumber = "" + (object) (char) (nNumber % 10 + 48) + this.m_bstrNumber;
        nNumber /= 10;
      }
      this.CheckForZeros();
    }

    public static BigNumber operator -(BigNumber num1, BigNumber num2)
    {
      string strNum = "";
      StringBuilder stringBuilder1 = new StringBuilder(num1.m_bstrNumber);
      StringBuilder stringBuilder2 = new StringBuilder(num2.m_bstrNumber);
      int length1 = stringBuilder1.Length;
      int length2 = stringBuilder2.Length;
      if (length2 < length1)
      {
        for (int index = 0; index < length1 - length2; ++index)
          stringBuilder2 = new StringBuilder("0" + (object) stringBuilder2);
      }
      for (int index1 = length1 - 1; index1 >= 0; --index1)
      {
        int num3 = (int) stringBuilder1[index1] - 48;
        int num4 = (int) stringBuilder2[index1] - 48;
        if (num3 >= num4)
        {
          strNum = "" + (object) (char) (num3 - num4 + 48) + strNum;
        }
        else
        {
          int index2 = index1 - 1;
          while (index2 >= 0 && (int) stringBuilder1[index2] == 48)
            stringBuilder1[index2--] = '9';
          --stringBuilder1[index2];
          strNum = "" + (object) (char) (num3 + 10 - num4 + 48) + strNum;
        }
      }
      return new BigNumber(strNum);
    }

    public static BigNumber operator +(BigNumber num1, BigNumber num2)
    {
      StringBuilder stringBuilder1 = new StringBuilder(num1.m_bstrNumber);
      StringBuilder stringBuilder2 = new StringBuilder(num2.m_bstrNumber);
      int length1 = stringBuilder1.Length;
      int length2 = stringBuilder2.Length;
      if (length1 > length2)
        stringBuilder2 = new StringBuilder(new string('0', length1 - length2) + (object) stringBuilder2);
      else
        stringBuilder1 = new StringBuilder(new string('0', length2 - length1) + (object) stringBuilder1);
      StringBuilder stringBuilder3 = new StringBuilder("0" + (object) stringBuilder1);
      StringBuilder stringBuilder4 = new StringBuilder("0" + (object) stringBuilder2);
      string strNum = "";
      int num3 = 0;
      for (int index = stringBuilder3.Length - 1; index >= 0; --index)
      {
        int num4 = (int) stringBuilder3[index] - 48 + ((int) stringBuilder4[index] - 48) + num3;
        strNum = "" + (object) (char) (num4 % 10 + 48) + strNum;
        num3 = num4 / 10;
      }
      return new BigNumber(strNum);
    }

    public static string ComputeBarcode(string Tracking, string Routing)
    {
      if (Tracking.Length != 20)
        return "";
      int length = Routing.Length;
      switch (length)
      {
        case 0:
        case 5:
        case 9:
        case 11:
          if ((int) Tracking[1] < 48 || (int) Tracking[1] > 52)
            return "";
          BigNumber bigNumber1 = new BigNumber(Routing);
          switch (length)
          {
            case 5:
              bigNumber1 += new BigNumber("1");
              break;
            case 9:
              bigNumber1 += new BigNumber("100001");
              break;
            case 11:
              bigNumber1 += new BigNumber("1000100001");
              break;
          }
          bigNumber1.Multiply(10);
          BigNumber bigNumber2 = bigNumber1 + new BigNumber((int) Tracking[0] - 48);
          bigNumber2.Multiply(5);
          BigNumber bigNumber3 = bigNumber2 + new BigNumber((int) Tracking[1] - 48);
          bigNumber3.Append(Tracking.Substring(2));
          bigNumber3.ToBytes();
          int num = (int) bigNumber3.USPS_MSB_Math_CRC11GenerateFrameCheckSequence();
          return bigNumber3.ToCodewords();
        default:
          return "";
      }
    }

    private void Multiply(int nFactor)
    {
      if (this.m_bZero || nFactor < 0 || nFactor > 10)
        return;
      if (nFactor == 10)
      {
        BigNumber bigNumber = this;
        string str = bigNumber.m_bstrNumber + "0";
        bigNumber.m_bstrNumber = str;
      }
      else
      {
        int num1 = 0;
        char[] chArray1 = new char[50];
        int num2 = 0;
        for (int index = this.m_bstrNumber.Length - 1; index >= 0; --index)
        {
          int num3 = ((int) this.m_bstrNumber[index] - 48) * nFactor + num1;
          num1 = num3 / 10;
          int num4 = num3 % 10;
          chArray1[num2++] = (char) (num4 + 48);
        }
        char[] chArray2 = chArray1;
        int index1 = num2;
        int num5 = 1;
        int num6 = index1 + num5;
        int num7 = (int) (ushort) (num1 + 48);
        chArray2[index1] = (char) num7;
        this.m_bstrNumber = "";
        for (int index2 = num6 - 1; index2 >= 0; --index2)
        {
          BigNumber bigNumber = this;
          string str = bigNumber.m_bstrNumber + (object) chArray1[index2];
          bigNumber.m_bstrNumber = str;
        }
        this.CheckForZeros();
      }
    }

    private void AppendDigit(int nDigit)
    {
      char ch = (char) (48 + nDigit % 10);
      if (this.m_bZero)
        this.m_bstrNumber = "";
      BigNumber bigNumber = this;
      string str = bigNumber.m_bstrNumber + (object) ch;
      bigNumber.m_bstrNumber = str;
      this.CheckForZeros();
    }

    private ulong ToNumber()
    {
      ulong num = 0UL;
      for (int index = 0; index < this.m_bstrNumber.Length; ++index)
        num = 10UL * num + (ulong) ((int) this.m_bstrNumber[index] - 48);
      return num;
    }

    public static BigNumber Divide(BigNumber Dividend, int nDivisor, ref BigNumber Remainder)
    {
      string strNum = "";
      BigNumber bigNumber1 = new BigNumber(Dividend);
      int index = 0;
      BigNumber bigNumber2;
      for (bigNumber2 = new BigNumber(); (int) bigNumber2.ToNumber() < nDivisor && index < Dividend.m_bstrNumber.Length; ++index)
        bigNumber2.AppendDigit((int) Dividend.m_bstrNumber[index] - 48);
      for (; index <= Dividend.m_bstrNumber.Length; bigNumber2.AppendDigit((int) Dividend.m_bstrNumber[index++] - 48))
      {
        char ch = (char) (bigNumber2.ToNumber() / (ulong) nDivisor + 48UL);
        strNum = strNum + (object) ch;
        BigNumber bigNumber3 = new BigNumber((int) bigNumber2.ToNumber() / nDivisor * nDivisor);
        bigNumber2 -= bigNumber3;
        if (index >= Dividend.m_bstrNumber.Length)
        {
          Remainder = bigNumber2;
          break;
        }
      }
      return new BigNumber(strNum);
    }

    private void ToBytes()
    {
      BigNumber Dividend = new BigNumber(this);
      BigNumber Remainder = new BigNumber();
      int num1 = 12;
      while (!Dividend.m_bZero)
      {
        Dividend = BigNumber.Divide(Dividend, 256, ref Remainder);
        int num2 = (int) Remainder.ToNumber();
        this.m_arrBytes[num1--] = (byte) num2;
        if (num1 < 0)
          break;
      }
    }

    private ushort USPS_MSB_Math_CRC11GenerateFrameCheckSequence()
    {
      int index1 = 0;
      ushort num1 = (ushort) 3893;
      ushort num2 = (ushort) 2047;
      ushort num3 = (ushort) ((uint) this.m_arrBytes[index1] << 5);
      int index2 = index1 + 1;
      for (int index3 = 2; index3 < 8; ++index3)
      {
        num2 = (ushort) (((((int) num2 ^ (int) num3) & 1024) == 0 ? (uint) (ushort) ((uint) num2 << 1) : (uint) (ushort) ((uint) num2 << 1 ^ (uint) num1)) & 2047U);
        num3 <<= 1;
      }
      for (int index3 = 1; index3 < 13; ++index3)
      {
        ushort num4 = (ushort) ((uint) this.m_arrBytes[index2] << 3);
        ++index2;
        for (int index4 = 0; index4 < 8; ++index4)
        {
          num2 = (ushort) (((((int) num2 ^ (int) num4) & 1024) == 0 ? (uint) (ushort) ((uint) num2 << 1) : (uint) (ushort) ((uint) num2 << 1 ^ (uint) num1)) & 2047U);
          num4 <<= 1;
        }
      }
      this.m_nFCS = num2;
      return num2;
    }

    private string ToCodewords()
    {
      int[] numArray1 = new int[10];
      int num1 = 9;
      BigNumber Dividend1 = new BigNumber(this);
      BigNumber Remainder = new BigNumber();
      BigNumber Dividend2 = BigNumber.Divide(Dividend1, 636, ref Remainder);
      int[] numArray2 = numArray1;
      int index1 = num1;
      int num2 = 1;
      int num3 = index1 - num2;
      int num4 = (int) Remainder.ToNumber();
      numArray2[index1] = num4;
      for (int index2 = 0; index2 < 8; ++index2)
      {
        Dividend2 = BigNumber.Divide(Dividend2, 1365, ref Remainder);
        numArray1[num3--] = (int) Remainder.ToNumber();
      }
      int[] numArray3 = numArray1;
      int index3 = num3;
      int num5 = 1;
      int num6 = index3 - num5;
      int num7 = (int) Dividend2.ToNumber();
      numArray3[index3] = num7;
      numArray1[9] *= 2;
      if (((int) this.m_nFCS & 1024) != 0)
        numArray1[0] += 659;
      ushort[] numArray4 = new ushort[10];
      ushort num8 = this.m_nFCS;
      for (int index2 = 0; index2 < 10; ++index2)
      {
        numArray4[index2] = numArray1[index2] > 1286 ? (ushort) BigNumber.m_arrTable2[numArray1[index2] - 1287] : (ushort) BigNumber.m_arrTable1[numArray1[index2]];
        if (((int) num8 & 1) != 0)
          numArray4[index2] = (ushort) ((uint) ~numArray4[index2] & 8191U);
        num8 >>= 1;
      }
      StringBuilder stringBuilder = new StringBuilder();
      for (int index2 = 0; index2 < 65; ++index2)
      {
        bool flag1 = ((int) numArray4[BigNumber.m_arrTable4[index2][0] - 65] & 1 << BigNumber.m_arrTable4[index2][1]) != 0;
        bool flag2 = ((int) numArray4[BigNumber.m_arrTable4[index2][2] - 65] & 1 << BigNumber.m_arrTable4[index2][3]) != 0;
        if (flag2 && flag1)
          stringBuilder.Append("F");
        else if (!flag2 && !flag1)
          stringBuilder.Append("T");
        else if (flag2 && !flag1)
          stringBuilder.Append("A");
        else
          stringBuilder.Append("D");
      }
      return ((object) stringBuilder).ToString();
    }

    private void Append(string sz)
    {
      BigNumber bigNumber = this;
      string str = bigNumber.m_bstrNumber + sz;
      bigNumber.m_bstrNumber = str;
    }

    private void CheckForZeros()
    {
      this.m_bZero = true;
      if (this.m_bstrNumber != null)
      {
        for (int index = 0; index < this.m_bstrNumber.Length; ++index)
        {
          if ((int) this.m_bstrNumber[index] != 48)
            this.m_bZero = false;
        }
      }
      if (this.m_bZero)
      {
        this.m_bstrNumber = "0";
      }
      else
      {
        int startIndex = 0;
        while ((int) this.m_bstrNumber[startIndex] == 48)
          ++startIndex;
        if (startIndex <= 0)
          return;
        this.m_bstrNumber = this.m_bstrNumber.Substring(startIndex);
      }
    }
  }
}
