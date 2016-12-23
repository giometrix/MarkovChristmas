using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarkovSharp.TokenisationStrategies;
using Sanford.Multimedia.Midi;

namespace Markov
{
	internal class Program
	{
		private const string OUTPUT_FILE_NAME = "MarkovChristmas.mid";

		private static List<Sequence> GetMidis()
		{
			var midiFiles = Directory.GetFiles("Songs");
			var midis = new List<Sequence>();
			foreach (var file in midiFiles)
				if (!file.EndsWith(OUTPUT_FILE_NAME))
				{
					var seq = new Sequence(file);

					midis.Add(seq);
				}
			return midis;
		}

		private static int GetDivisionForNewMidi(List<Sequence> midis)
		{
			var model = new StringMarkov(1);
			model.Learn(midis.Select(m => m.Division.ToString()));
			return int.Parse(model.Walk().Single());
		}


		private static void Main(string[] args)
		{
			var midis = GetMidis();

			var newSeq = new Sequence(GetDivisionForNewMidi(midis));


			var model = new SanfordMidiMarkov(2);

			LearnTracks(midis, model);


			newSeq.Add(model.Walk().FirstOrDefault());


			newSeq.Save($"Songs/{OUTPUT_FILE_NAME}");

			GenerateLyrics();
			Console.ReadKey();
		}

		private static void LearnTracks(List<Sequence> midis, SanfordMidiMarkov model)
		{
			foreach (var midi in midis)
			{
				var track = new Track();
				foreach (var t in midi)
					track.Merge(t);
				model.Learn(track);
			}
		}

		private static void GenerateLyrics()
		{
			var phrases = new List<string>
			{
				"I saw Mommy kissing Santa Claus I saw Mommy kissing Santa Claus",
				"She didn't see me creep down the stairs to have a peep",
				"She thought that I was tucked up in my bedroom fast asleep",
				"Then I saw Mommy tickle Santa Claus Underneath his beard so snowy white",
				"Oh what a laugh it would have been If Daddy had only seen Mommy kissing Santa Claus last night",
				"Deck the halls with boughs of holly, Fa la la la la la la la",
				"Tis the season to be jolly, Fa la la la la la la la!",
				"Don we now our gay apparel, Fa la la la la la la la!",
				"Troll the ancient Yuletide carol, Fa la la la la la la la",
				"See the blazing yule before us, Fa la la la la la la la!",
				"Strike the harp and join the chorus, Fa la la la la la la la",
				"Follow me in merry measure, Fa la la la la la la la",
				"While I tell of Yuletide treasure, Fa la la la la la la la",
				"Fast away the old year passes, Fa la la la la la la la",
				"Hail the new, ye lads and lasses, Fa la la la la la la la",
				"Sing we joyous all together! Fa la la la la la la la",
				"Heedless of the wind and weather, Fa la la la la la la la",
				"You know Dasher and Dancer and",
				"Prancer and Vixen",
				"Comet and Cupid and",
				"Donder and Blitzen",
				"But do you recall",
				"The most famous reindeer of all",
				"Rudolph the red-nosed reindeer",
				"had a very shiny nose",
				"and if you ever saw it",
				"you would even say it glows",
				"All of the other reindeer",
				"used to laugh and call him names",
				"They never let poor Rudolph",
				"play in any reindeer games",
				"Then one foggy Christmas eve",
				"Santa came to say",
				"Rudolph with your nose so bright",
				"won't you guide my sleigh tonight",
				"Then all the reindeer loved him",
				"as they shouted out with glee",
				"Rudolph the red-nosed reindeer",
				"you'll go down in history",
				"It's the most wonderful time of the year ",
				"With the kids jingle belling",
				"And everyone telling you 'Be of good cheer'",
				"It's the most wonderful time of the year",
				"It's the hap-happiest season of all",
				"With those holiday greetings and gay happy meetings",
				"When friends come to call",
				"It's the hap- happiest season of all",
				"There'll be parties for hosting",
				"Marshmallows for toasting",
				"And caroling out in the snow",
				"There'll be scary ghost stories",
				"And tales of the glories of",
				"Christmases long, long ago",
				"It's the most wonderful time of the year",
				"There'll be much mistletoeing",
				"And hearts will be glowing",
				"When love ones are near",
				"It's the most wonderful time of the year",
				"There'll be parties for hosting",
				"Marshmallows for toasting",
				"And caroling out in the snow",
				"There'll be scary ghost stories",
				"And tales of the glories of",
				"Christmases long, long ago",
				"It's the most wonderful time of the year",
				"There'll be much mistletoeing",
				"And hearts will be glowing",
				"When love ones are near",
				"It's the most wonderful time",
				"It's the most wonderful time of the year",
				"Here comes Santa Claus, here comes Santa Claus,",
				"Right down Santa Claus lane",
				"Vixen and Blitzen and all his reindeer",
				"Pullin' on the reins",
				"Bells are ringin', children singin'",
				"All is merry and bright",
				"Hang your stockings and say your prayers",
				"'Cause Santa Claus comes tonight!",
				"Here comes Santa Claus, here comes Santa Claus,",
				"Right down Santa Claus lane",
				"He's got a bag that's filled with toys",
				"For boys and girls again",
				"Hear those sleigh bells jingle jangle,",
				"Oh what a beautiful sight",
				"So jump in bed and cover your head",
				"'Cause Santa Claus comes tonight!",
				"Here comes Santa Claus, here comes Santa Claus,",
				"Right down Santa Claus lane",
				"He doesn't care if you're rich or poor",
				"He loves you just the same",
				"Santa Claus knows we're all Gods children",
				"That makes everything right",
				"So fill your hearts with Christmas cheer",
				"'Cause Santa Claus comes tonight!",
				"Here comes Santa Claus, here comes Santa Claus,",
				"Right down Santa Claus lane",
				"He'll come around when the chimes ring out",
				"That it's Christmas morn again",
				"Peace on earth will come to all",
				"If we just follow the light",
				"So lets give thanks to the lord above",
				"That Santa Claus comes tonight!",
				"O Christmas tree, O Christmas tree!",
				"How are thy leaves so verdant!",
				"O Christmas tree, O Christmas tree,",
				"How are thy leaves so verdant!",
				"Not only in the summertime,",
				"But even in winter is thy prime.",
				"O Christmas tree, O Christmas tree,",
				"How are thy leaves so verdant!",
				"O Christmas tree, O Christmas tree,",
				"Much pleasure doth thou bring me!",
				"O Christmas tree, O Christmas tree,",
				"Much pleasure doth thou bring me!",
				"For every year the Christmas tree,",
				"Brings to us all both joy and glee.",
				"O Christmas tree, O Christmas tree,",
				"Much pleasure doth thou bring me!",
				"O Christmas tree, O Christmas tree,",
				"Thy candles shine out brightly!",
				"O Christmas tree, O Christmas tree,",
				"Thy candles shine out brightly!",
				"Each bough doth hold its tiny light,",
				"That makes each toy to sparkle bright.",
				"O Christmas tree, O Christmas tree,",
				"Thy candles shine out brightly!",
				"I'm dreaming of a White Christmas",
				"Just like the ones I used to know",
				"Where the treetops glisten",
				"and children listen",
				"To hear sleigh bells in the snow.",
				"I'm dreaming of a white Christmas",
				"With every Christmas card I write",
				"May your days be merry and bright",
				"And may all your Christmases be white.",
				"I'm dreaming of a white Christmas",
				"With every Christmas card I write",
				"May your days be merry and bright",
				"And may all your Christmases be white.",
				"I'll be home for Christmas",
				"You can plan on me",
				"Please have snow and mistletoe",
				"And presents on the tree",
				"Christmas Eve will find me",
				"Where the love light gleams",
				"I'll be home for Christmas",
				"If only in my dreams",
				"I'll be home for Christmas",
				"You can plan on me",
				"Please have snow and mistletoe",
				"And presents on the tree",
				"Christmas Eve will find me",
				"Where the love light gleams",
				"I'll be home for Christmas",
				"If only in my dreams",
				"If only in my dreams"
			};


			// Create a new model
			var model = new StringMarkov(1);

			// Train the model
			model.Learn(phrases);


			for (var i = 0; i < 3; i++)
			{
				for (var j = 0; j < 6; j++)
					Console.WriteLine(model.Walk().First());
				Console.WriteLine();
			}
		}
	}
}