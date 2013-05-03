using System;
namespace Persits.PDF
{
	internal class PdfArabic
	{
		private const char ALEF = 'ا';
		private const char ALEFHAMZA = 'أ';
		private const char ALEFHAMZABELOW = 'إ';
		private const char ALEFMADDA = 'آ';
		private const char LAM = 'ل';
		private const char HAMZA = 'ء';
		private const char TATWEEL = 'ـ';
		private const char ZWJ = '‍';
		private const char HAMZAABOVE = 'ٔ';
		private const char HAMZABELOW = 'ٕ';
		private const char WAWHAMZA = 'ؤ';
		private const char YEHHAMZA = 'ئ';
		private const char WAW = 'و';
		private const char ALEFMAKSURA = 'ى';
		private const char YEH = 'ي';
		private const char FARSIYEH = 'ی';
		private const char SHADDA = 'ّ';
		private const char KASRA = 'ِ';
		private const char FATHA = 'َ';
		private const char DAMMA = 'ُ';
		private const char MADDA = 'ٓ';
		private const char LAM_ALEF = 'ﻻ';
		private const char LAM_ALEFHAMZA = 'ﻷ';
		private const char LAM_ALEFHAMZABELOW = 'ﻹ';
		private const char LAM_ALEFMADDA = 'ﻵ';
		private static shapestruct[] chartable = new shapestruct[]
		{
			new shapestruct(1569u, 1, new uint[]
			{
				65152u
			}),
			new shapestruct(1570u, 2, new uint[]
			{
				65153u,
				65154u
			}),
			new shapestruct(1571u, 2, new uint[]
			{
				65155u,
				65156u
			}),
			new shapestruct(1572u, 2, new uint[]
			{
				65157u,
				65158u
			}),
			new shapestruct(1573u, 2, new uint[]
			{
				65159u,
				65160u
			}),
			new shapestruct(1574u, 4, new uint[]
			{
				65161u,
				65162u,
				65163u,
				65164u
			}),
			new shapestruct(1575u, 2, new uint[]
			{
				65165u,
				65166u
			}),
			new shapestruct(1576u, 4, new uint[]
			{
				65167u,
				65168u,
				65169u,
				65170u
			}),
			new shapestruct(1577u, 2, new uint[]
			{
				65171u,
				65172u
			}),
			new shapestruct(1578u, 4, new uint[]
			{
				65173u,
				65174u,
				65175u,
				65176u
			}),
			new shapestruct(1579u, 4, new uint[]
			{
				65177u,
				65178u,
				65179u,
				65180u
			}),
			new shapestruct(1580u, 4, new uint[]
			{
				65181u,
				65182u,
				65183u,
				65184u
			}),
			new shapestruct(1581u, 4, new uint[]
			{
				65185u,
				65186u,
				65187u,
				65188u
			}),
			new shapestruct(1582u, 4, new uint[]
			{
				65189u,
				65190u,
				65191u,
				65192u
			}),
			new shapestruct(1583u, 2, new uint[]
			{
				65193u,
				65194u
			}),
			new shapestruct(1584u, 2, new uint[]
			{
				65195u,
				65196u
			}),
			new shapestruct(1585u, 2, new uint[]
			{
				65197u,
				65198u
			}),
			new shapestruct(1586u, 2, new uint[]
			{
				65199u,
				65200u
			}),
			new shapestruct(1587u, 4, new uint[]
			{
				65201u,
				65202u,
				65203u,
				65204u
			}),
			new shapestruct(1588u, 4, new uint[]
			{
				65205u,
				65206u,
				65207u,
				65208u
			}),
			new shapestruct(1589u, 4, new uint[]
			{
				65209u,
				65210u,
				65211u,
				65212u
			}),
			new shapestruct(1590u, 4, new uint[]
			{
				65213u,
				65214u,
				65215u,
				65216u
			}),
			new shapestruct(1591u, 4, new uint[]
			{
				65217u,
				65218u,
				65219u,
				65220u
			}),
			new shapestruct(1592u, 4, new uint[]
			{
				65221u,
				65222u,
				65223u,
				65224u
			}),
			new shapestruct(1593u, 4, new uint[]
			{
				65225u,
				65226u,
				65227u,
				65228u
			}),
			new shapestruct(1594u, 4, new uint[]
			{
				65229u,
				65230u,
				65231u,
				65232u
			}),
			new shapestruct(1600u, 4, new uint[]
			{
				1600u,
				1600u,
				1600u,
				1600u
			}),
			new shapestruct(1601u, 4, new uint[]
			{
				65233u,
				65234u,
				65235u,
				65236u
			}),
			new shapestruct(1602u, 4, new uint[]
			{
				65237u,
				65238u,
				65239u,
				65240u
			}),
			new shapestruct(1603u, 4, new uint[]
			{
				65241u,
				65242u,
				65243u,
				65244u
			}),
			new shapestruct(1604u, 4, new uint[]
			{
				65245u,
				65246u,
				65247u,
				65248u
			}),
			new shapestruct(1605u, 4, new uint[]
			{
				65249u,
				65250u,
				65251u,
				65252u
			}),
			new shapestruct(1606u, 4, new uint[]
			{
				65253u,
				65254u,
				65255u,
				65256u
			}),
			new shapestruct(1607u, 4, new uint[]
			{
				65257u,
				65258u,
				65259u,
				65260u
			}),
			new shapestruct(1608u, 2, new uint[]
			{
				65261u,
				65262u
			}),
			new shapestruct(1609u, 4, new uint[]
			{
				65263u,
				65264u,
				64488u,
				64489u
			}),
			new shapestruct(1610u, 4, new uint[]
			{
				65265u,
				65266u,
				65267u,
				65268u
			}),
			new shapestruct(1649u, 2, new uint[]
			{
				64336u,
				64337u
			}),
			new shapestruct(1657u, 4, new uint[]
			{
				64358u,
				64359u,
				64360u,
				64361u
			}),
			new shapestruct(1658u, 4, new uint[]
			{
				64350u,
				64351u,
				64352u,
				64353u
			}),
			new shapestruct(1659u, 4, new uint[]
			{
				64338u,
				64339u,
				64340u,
				64341u
			}),
			new shapestruct(1662u, 4, new uint[]
			{
				64342u,
				64343u,
				64344u,
				64345u
			}),
			new shapestruct(1663u, 4, new uint[]
			{
				64354u,
				64355u,
				64356u,
				64357u
			}),
			new shapestruct(1664u, 4, new uint[]
			{
				64346u,
				64347u,
				64348u,
				64349u
			}),
			new shapestruct(1667u, 4, new uint[]
			{
				64374u,
				64375u,
				64376u,
				64377u
			}),
			new shapestruct(1668u, 4, new uint[]
			{
				64370u,
				64371u,
				64372u,
				64373u
			}),
			new shapestruct(1670u, 4, new uint[]
			{
				64378u,
				64379u,
				64380u,
				64381u
			}),
			new shapestruct(1671u, 4, new uint[]
			{
				64382u,
				64383u,
				64384u,
				64385u
			}),
			new shapestruct(1672u, 2, new uint[]
			{
				64392u,
				64393u
			}),
			new shapestruct(1676u, 2, new uint[]
			{
				64388u,
				64389u
			}),
			new shapestruct(1677u, 2, new uint[]
			{
				64386u,
				64387u
			}),
			new shapestruct(1678u, 2, new uint[]
			{
				64390u,
				64391u
			}),
			new shapestruct(1681u, 2, new uint[]
			{
				64396u,
				64397u
			}),
			new shapestruct(1688u, 2, new uint[]
			{
				64394u,
				64395u
			}),
			new shapestruct(1700u, 4, new uint[]
			{
				64362u,
				64363u,
				64364u,
				64365u
			}),
			new shapestruct(1702u, 4, new uint[]
			{
				64366u,
				64367u,
				64368u,
				64369u
			}),
			new shapestruct(1705u, 4, new uint[]
			{
				64398u,
				64399u,
				64400u,
				64401u
			}),
			new shapestruct(1709u, 4, new uint[]
			{
				64467u,
				64468u,
				64469u,
				64470u
			}),
			new shapestruct(1711u, 4, new uint[]
			{
				64402u,
				64403u,
				64404u,
				64405u
			}),
			new shapestruct(1713u, 4, new uint[]
			{
				64410u,
				64411u,
				64412u,
				64413u
			}),
			new shapestruct(1715u, 4, new uint[]
			{
				64406u,
				64407u,
				64408u,
				64409u
			}),
			new shapestruct(1723u, 4, new uint[]
			{
				64416u,
				64417u,
				64418u,
				64419u
			}),
			new shapestruct(1726u, 4, new uint[]
			{
				64426u,
				64427u,
				64428u,
				64429u
			}),
			new shapestruct(1728u, 2, new uint[]
			{
				64420u,
				64421u
			}),
			new shapestruct(1729u, 4, new uint[]
			{
				64422u,
				64423u,
				64424u,
				64425u
			}),
			new shapestruct(1733u, 2, new uint[]
			{
				64480u,
				64481u
			}),
			new shapestruct(1734u, 2, new uint[]
			{
				64473u,
				64474u
			}),
			new shapestruct(1735u, 2, new uint[]
			{
				64471u,
				64472u
			}),
			new shapestruct(1736u, 2, new uint[]
			{
				64475u,
				64476u
			}),
			new shapestruct(1737u, 2, new uint[]
			{
				64482u,
				64483u
			}),
			new shapestruct(1739u, 2, new uint[]
			{
				64478u,
				64479u
			}),
			new shapestruct(1740u, 4, new uint[]
			{
				64508u,
				64509u,
				64510u,
				64511u
			}),
			new shapestruct(1744u, 4, new uint[]
			{
				64484u,
				64485u,
				64486u,
				64487u
			}),
			new shapestruct(1746u, 2, new uint[]
			{
				64430u,
				64431u
			}),
			new shapestruct(1747u, 2, new uint[]
			{
				64432u,
				64433u
			})
		};
		private static uint[] unshapetableFB = new uint[]
		{
			1649u,
			1649u,
			1659u,
			1659u,
			1659u,
			1659u,
			1662u,
			1662u,
			1662u,
			1662u,
			1664u,
			1664u,
			1664u,
			1664u,
			1658u,
			1658u,
			1658u,
			1658u,
			1663u,
			1663u,
			1663u,
			1663u,
			1657u,
			1657u,
			1657u,
			1657u,
			1700u,
			1700u,
			1700u,
			1700u,
			1702u,
			1702u,
			1702u,
			1702u,
			1668u,
			1668u,
			1668u,
			1668u,
			1667u,
			1667u,
			1667u,
			1667u,
			1670u,
			1670u,
			1670u,
			1670u,
			1671u,
			1671u,
			1671u,
			1671u,
			1677u,
			1677u,
			1676u,
			1676u,
			1678u,
			1678u,
			1672u,
			1672u,
			1688u,
			1688u,
			1681u,
			1681u,
			1705u,
			1705u,
			1705u,
			1705u,
			1711u,
			1711u,
			1711u,
			1711u,
			1715u,
			1715u,
			1715u,
			1715u,
			1713u,
			1713u,
			1713u,
			1713u,
			0u,
			0u,
			1723u,
			1723u,
			1723u,
			1723u,
			1728u,
			1728u,
			1729u,
			1729u,
			1729u,
			1729u,
			1726u,
			1726u,
			1726u,
			1726u,
			1746u,
			1746u,
			1747u,
			1747u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			1709u,
			1709u,
			1709u,
			1709u,
			1735u,
			1735u,
			1734u,
			1734u,
			1736u,
			1736u,
			0u,
			1739u,
			1739u,
			1733u,
			1733u,
			1737u,
			1737u,
			1744u,
			1744u,
			1744u,
			1744u,
			1609u,
			1609u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			0u,
			1740u,
			1740u,
			1740u,
			1740u
		};
		private static uint[] unshapetableFE = new uint[]
		{
			1569u,
			1570u,
			1570u,
			1571u,
			1571u,
			1572u,
			1572u,
			1573u,
			1573u,
			1574u,
			1574u,
			1574u,
			1574u,
			1575u,
			1575u,
			1576u,
			1576u,
			1576u,
			1576u,
			1577u,
			1577u,
			1578u,
			1578u,
			1578u,
			1578u,
			1579u,
			1579u,
			1579u,
			1579u,
			1580u,
			1580u,
			1580u,
			1580u,
			1581u,
			1581u,
			1581u,
			1581u,
			1582u,
			1582u,
			1582u,
			1582u,
			1583u,
			1583u,
			1584u,
			1584u,
			1585u,
			1585u,
			1586u,
			1586u,
			1587u,
			1587u,
			1587u,
			1587u,
			1588u,
			1588u,
			1588u,
			1588u,
			1589u,
			1589u,
			1589u,
			1589u,
			1590u,
			1590u,
			1590u,
			1590u,
			1591u,
			1591u,
			1591u,
			1591u,
			1592u,
			1592u,
			1592u,
			1592u,
			1593u,
			1593u,
			1593u,
			1593u,
			1594u,
			1594u,
			1594u,
			1594u,
			1601u,
			1601u,
			1601u,
			1601u,
			1602u,
			1602u,
			1602u,
			1602u,
			1603u,
			1603u,
			1603u,
			1603u,
			1604u,
			1604u,
			1604u,
			1604u,
			1605u,
			1605u,
			1605u,
			1605u,
			1606u,
			1606u,
			1606u,
			1606u,
			1607u,
			1607u,
			1607u,
			1607u,
			1608u,
			1608u,
			1609u,
			1609u,
			1610u,
			1610u,
			1610u,
			1610u
		};
		private static bool connects_to_left(charstruct a)
		{
			return a.numshapes > 2;
		}
		private static void charstruct_init(charstruct s)
		{
			s.basechar = '\0';
			s.mark1 = '\0';
			s.vowel = '\0';
			s.lignum = 0;
			s.numshapes = 1;
		}
		private static void copycstostring(char[] objString, ref int i, charstruct s, arabic_level level)
		{
			if (s.basechar == '\0')
			{
				return;
			}
			objString[i] = s.basechar;
			i++;
			s.lignum -= 1;
			if (s.mark1 != '\0')
			{
				if ((level & arabic_level.ar_novowel) == arabic_level.ar_nothing)
				{
					objString[i] = s.mark1;
					i++;
					s.lignum -= 1;
				}
				else
				{
					s.lignum -= 1;
				}
			}
			if (s.vowel != '\0')
			{
				if ((level & arabic_level.ar_novowel) == arabic_level.ar_nothing)
				{
					objString[i] = s.vowel;
					i++;
					s.lignum -= 1;
				}
				else
				{
					s.lignum -= 1;
				}
			}
			while (s.lignum > 0)
			{
				objString[i] = '\0';
				i++;
				s.lignum -= 1;
			}
		}
		private static int arabic_isvowel(char s)
		{
			if ((s >= 'ً' && s <= 'ٕ') || s == 'ٰ')
			{
				return 1;
			}
			return 0;
		}
		private static char unshape(char s)
		{
			if (s >= 'ﺀ' && s <= 'ﻴ')
			{
				return (char)PdfArabic.unshapetableFE[(int)(s - 'ﺀ')];
			}
			if (s >= 'ﻵ' && s <= 'ﻼ')
			{
				if (s % '\u0002' <= '\0')
				{
                    return (char)((uint)s - 1U);
				}
				return s;
			}
			else
			{
				if (s < 'ﭐ' || s > 'ﯿ')
				{
					return s;
				}
				char result;
				if ((result = (char)PdfArabic.unshapetableFB[(int)(s - 'ﭐ')]) == '\0')
				{
					return s;
				}
				return result;
			}
		}
		private static char charshape(char s, short which)
		{
            if ((int)s >= 1569 && (int)s <= 1747)
			{
				int i = 0;
				int num = PdfArabic.chartable.Length;
				while (i <= num)
				{
					int num2 = (i + num) / 2;
					if (s == PdfArabic.chartable[num2].basechar)
					{
						return PdfArabic.chartable[num2].charshape[(int)which];
					}
					if (s < PdfArabic.chartable[num2].basechar)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
            else if ((int)s >= 65269 && (int)s <= 65275)
                return (char)((uint)s + (uint)which);
            return s;
		}
		private static short shapecount(char s)
		{
			if (s >= 'ء' && s <= 'ۓ' && PdfArabic.arabic_isvowel(s) == 0)
			{
				int i = 0;
				int num = PdfArabic.chartable.Length;
				while (i <= num)
				{
					int num2 = (i + num) / 2;
					if (s == PdfArabic.chartable[num2].basechar)
					{
						return (short)PdfArabic.chartable[num2].count;
					}
					if (s < PdfArabic.chartable[num2].basechar)
					{
						num = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
			else
			{
				if (s == '‍')
				{
					return 4;
				}
			}
			return 1;
		}
		private static int unligature(charstruct curchar, arabic_level level)
		{
			int num = 0;
			if ((level & arabic_level.ar_naqshfont) != arabic_level.ar_nothing)
			{
				switch (curchar.basechar)
				{
				case 'آ':
					curchar.basechar = 'ا';
					curchar.vowel = 'ٓ';
					num++;
					break;
				case 'أ':
					curchar.basechar = 'ا';
					curchar.mark1 = 'ٔ';
					num++;
					break;
				case 'ؤ':
					curchar.basechar = 'و';
					curchar.mark1 = 'ٔ';
					num++;
					break;
				case 'إ':
					curchar.basechar = 'ا';
					curchar.mark1 = 'ٕ';
					num++;
					break;
				}
			}
			return num;
		}
		private static int ligature(char newchar, charstruct oldchar)
		{
			int num = 0;
			if (oldchar.basechar == '\0')
			{
				return 0;
			}
			if (PdfArabic.arabic_isvowel(newchar) != 0)
			{
				num = 1;
				if (oldchar.vowel != '\0' && newchar != 'ّ')
				{
					num = 2;
				}
				switch (newchar)
				{
				case 'ّ':
					if (oldchar.mark1 == '\0')
					{
						oldchar.mark1 = 'ّ';
						goto IL_16C;
					}
					return 0;
				case 'ٓ':
				{
					char basechar = oldchar.basechar;
					if (basechar == 'ا')
					{
						oldchar.basechar = 'آ';
						num = 2;
						goto IL_16C;
					}
					goto IL_16C;
				}
				case 'ٔ':
				{
					char basechar2 = oldchar.basechar;
					if (basechar2 <= 'ي')
					{
						if (basechar2 == 'ا')
						{
							oldchar.basechar = 'أ';
							num = 2;
							goto IL_16C;
						}
						switch (basechar2)
						{
						case 'و':
							oldchar.basechar = 'ؤ';
							num = 2;
							goto IL_16C;
						case 'ى':
						case 'ي':
							break;
						default:
							goto IL_138;
						}
					}
					else
					{
						if (basechar2 != 'ی')
						{
							if (basechar2 != 'ﻻ')
							{
								goto IL_138;
							}
							oldchar.basechar = 'ﻷ';
							num = 2;
							goto IL_16C;
						}
					}
					oldchar.basechar = 'ئ';
					num = 2;
					goto IL_16C;
					IL_138:
					oldchar.mark1 = 'ٔ';
					goto IL_16C;
				}
				case 'ٕ':
				{
					char basechar3 = oldchar.basechar;
					if (basechar3 == 'ا')
					{
						oldchar.basechar = 'إ';
						num = 2;
						goto IL_16C;
					}
					if (basechar3 != 'ﻻ')
					{
						oldchar.mark1 = 'ٕ';
						goto IL_16C;
					}
					oldchar.basechar = 'ﻹ';
					num = 2;
					goto IL_16C;
				}
				}
				oldchar.vowel = newchar;
				IL_16C:
				if (num == 1)
				{
					oldchar.lignum += 1;
				}
				return num;
			}
			if (oldchar.vowel != '\0')
			{
				return 0;
			}
			char basechar4 = oldchar.basechar;
			if (basechar4 != '\0')
			{
				if (basechar4 == 'ل')
				{
					switch (newchar)
					{
					case 'آ':
						oldchar.basechar = 'ﻵ';
						oldchar.numshapes = 2;
						num = 3;
						break;
					case 'أ':
						oldchar.basechar = 'ﻷ';
						oldchar.numshapes = 2;
						num = 3;
						break;
					case 'إ':
						oldchar.basechar = 'ﻹ';
						oldchar.numshapes = 2;
						num = 3;
						break;
					case 'ا':
						oldchar.basechar = 'ﻻ';
						oldchar.numshapes = 2;
						num = 3;
						break;
					}
				}
			}
			else
			{
				oldchar.basechar = newchar;
				oldchar.numshapes = (byte)PdfArabic.shapecount(newchar);
				num = 1;
			}
			return num;
		}
		private static void shape(ref int len, char[] text, char[] objString, arabic_level level)
		{
			int num = len;
			charstruct charstruct = new charstruct();
			charstruct charstruct2 = new charstruct();
			int num2 = 0;
			int i = 0;
			len = 0;
			PdfArabic.charstruct_init(charstruct);
			PdfArabic.charstruct_init(charstruct2);
			int num5;
			while (i < num)
			{
				char c = text[i];
				c = PdfArabic.unshape(c);
				int num3 = PdfArabic.ligature(c, charstruct2);
				if (num3 == 0)
				{
					int num4 = (int)PdfArabic.shapecount(c);
					len++;
					if (num4 == 1)
					{
						num5 = 0;
					}
					else
					{
						num5 = 2;
					}
					if (PdfArabic.connects_to_left(charstruct))
					{
						num5++;
					}
					num5 %= (int)charstruct2.numshapes;
					charstruct2.basechar = PdfArabic.charshape(charstruct2.basechar, (short)num5);
					PdfArabic.copycstostring(objString, ref num2, charstruct, level);
					charstruct.Set(charstruct2);
					PdfArabic.charstruct_init(charstruct2);
					charstruct2.basechar = c;
					charstruct2.numshapes = (byte)num4;
					charstruct expr_BA = charstruct2;
					expr_BA.lignum += 1;
					len += PdfArabic.unligature(charstruct2, level);
				}
				else
				{
					if (num3 == 3 && (level & arabic_level.ar_lboxfont) != arabic_level.ar_nothing)
					{
						len++;
						charstruct expr_E8 = charstruct2;
						expr_E8.lignum += 1;
					}
					else
					{
						if (num3 == 1)
						{
							len++;
						}
						else
						{
							len += PdfArabic.unligature(charstruct2, level);
						}
					}
				}
				i++;
			}
			if (PdfArabic.connects_to_left(charstruct))
			{
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			num5 %= (int)charstruct2.numshapes;
			charstruct2.basechar = PdfArabic.charshape(charstruct2.basechar, (short)num5);
			PdfArabic.copycstostring(objString, ref num2, charstruct, level);
			PdfArabic.copycstostring(objString, ref num2, charstruct2, level);
		}
		private static void doublelig(ref int len, char[] objString, arabic_level level)
		{
			int num = len;
			int num2 = 0;
			int i = 1;
			while (i < num)
			{
				ushort num3 = 0;
				if ((level & arabic_level.ar_composedtashkeel) != arabic_level.ar_nothing)
				{
					switch (objString[num2])
					{
					case 'َ':
						if (objString[i] == 'ّ')
						{
							num3 = 64608;
						}
						break;
					case 'ُ':
						if (objString[i] == 'ّ')
						{
							num3 = 64609;
						}
						break;
					case 'ِ':
						if (objString[i] == 'ّ')
						{
							num3 = 64610;
						}
						break;
					case 'ّ':
						switch (objString[i])
						{
						case 'ٌ':
							num3 = 64606;
							break;
						case 'ٍ':
							num3 = 64607;
							break;
						case 'َ':
							num3 = 64608;
							break;
						case 'ُ':
							num3 = 64609;
							break;
						case 'ِ':
							num3 = 64610;
							break;
						}
						break;
					}
				}
				if ((level & arabic_level.ar_lig) != arabic_level.ar_nothing)
				{
					ushort num4 = (ushort)objString[num2];
					if (num4 <= 65235)
					{
						if (num4 != 65169)
						{
							if (num4 != 65175)
							{
								if (num4 == 65235)
								{
									ushort num5 = (ushort)objString[i];
									if (num5 == 65266)
									{
										num3 = 64562;
									}
								}
							}
							else
							{
								ushort num6 = (ushort)objString[i];
								if (num6 != 65184)
								{
									if (num6 != 65188)
									{
										if (num6 == 65192)
										{
											num3 = 64675;
										}
									}
									else
									{
										num3 = 64674;
									}
								}
								else
								{
									num3 = 64673;
								}
							}
						}
						else
						{
							ushort num7 = (ushort)objString[i];
							if (num7 != 65184)
							{
								if (num7 != 65188)
								{
									if (num7 == 65192)
									{
										num3 = 64670;
									}
								}
								else
								{
									num3 = 64669;
								}
							}
							else
							{
								num3 = 64668;
							}
						}
					}
					else
					{
						if (num4 != 65247)
						{
							if (num4 != 65251)
							{
								switch (num4)
								{
								case 65255:
								{
									ushort num8 = (ushort)objString[i];
									if (num8 != 65184)
									{
										if (num8 != 65188)
										{
											if (num8 == 65192)
											{
												num3 = 64724;
											}
										}
										else
										{
											num3 = 64723;
										}
									}
									else
									{
										num3 = 64722;
									}
									break;
								}
								case 65256:
									switch (objString[i])
									{
									case 'ﺮ':
										num3 = 64650;
										break;
									case 'ﺰ':
										num3 = 64651;
										break;
									}
									break;
								}
							}
							else
							{
								ushort num9 = (ushort)objString[i];
								if (num9 <= 65188)
								{
									if (num9 != 65184)
									{
										if (num9 == 65188)
										{
											num3 = 64719;
										}
									}
									else
									{
										num3 = 64718;
									}
								}
								else
								{
									if (num9 != 65192)
									{
										if (num9 == 65252)
										{
											num3 = 64721;
										}
									}
									else
									{
										num3 = 64720;
									}
								}
							}
						}
						else
						{
							ushort num10 = (ushort)objString[i];
							switch (num10)
							{
							case 65182:
								num3 = 64575;
								break;
							case 65183:
							case 65185:
							case 65187:
							case 65189:
							case 65191:
								break;
							case 65184:
								num3 = 64713;
								break;
							case 65186:
								num3 = 64576;
								break;
							case 65188:
								num3 = 64714;
								break;
							case 65190:
								num3 = 64577;
								break;
							case 65192:
								num3 = 64715;
								break;
							default:
								switch (num10)
								{
								case 65250:
									num3 = 64578;
									break;
								case 65252:
									num3 = 64716;
									break;
								}
								break;
							}
						}
					}
				}
				if (num3 != 0)
				{
					objString[num2] = (char)num3;
					len--;
					i++;
				}
				else
				{
					num2++;
					objString[num2] = objString[i];
					i++;
				}
			}
		}
		internal void arabic_reshape(ref int len, char[] text, char[] objString, arabic_level level)
		{
			PdfArabic.shape(ref len, text, objString, level);
			if ((level & (arabic_level)12) != arabic_level.ar_nothing)
			{
				PdfArabic.doublelig(ref len, objString, level);
			}
		}
	}
}
