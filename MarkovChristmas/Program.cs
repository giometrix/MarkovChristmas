using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkovSharp.TokenisationStrategies;
using Sanford.Multimedia.Midi;



namespace Markov
{
	class Program
	{
		private const string OUTPUT_FILE_NAME = "MarkovChristmas.mid";
		private static List<Sequence> GetMidis()
		{
			var midiFiles = Directory.GetFiles("Songs");
			var midis = new List<Sequence>();
			foreach (var file in midiFiles)
			{
				if (!file.EndsWith(OUTPUT_FILE_NAME))
				{
					var seq = new Sequence(file);

					midis.Add(seq);
				}
			
			}
			return midis;
		}

		private static int GetDivisionForNewMidi(List<Sequence> midis)
		{
			var model = new StringMarkov(1);
			model.Learn(midis.Select(m => m.Division.ToString()));
			return int.Parse(model.Walk().Single());

		}

	
		static void Main(string[] args)
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
				{
					track.Merge(t);
				}
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
				"you'll go down in history"
			};


			// Create a new model
			var model = new StringMarkov(1);

			// Train the model
			model.Learn(phrases);


			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					Console.WriteLine(model.Walk().First());
					
				}
				Console.WriteLine();
			}
		
		}
	}
}
