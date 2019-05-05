using System;
using System.Collections.Generic;
using System.Linq;

namespace odbc_config
{
    class FuzzyMatcher
    {
        /// <summary>
        /// Contains information about a match candidate in the fuzzy finder.
        /// </summary>
        class FuzzyMatchState
        {
            public int Index { get; private set; }
            public int Score { get; private set; }

            public FuzzyMatchState(int index, int score)
            {
                Index = index;
                Score = score;
            }

            /// <summary>
            /// Deprioritizes this match candidate when a non-matching
            /// character is consumed.
            /// </summary>
            public void Penalize()
            {
                Score++;
            }

            /// <summary>
            /// Whether the match candidate has consumed any characters.
            /// </summary>
            public bool Started()
            {
                return Index > 0;
            }

            /// <summary>
            /// Whether the match candidate has consumed all the characters of
            /// the pattern.
            /// </summary>
            public bool IsDone(string pattern)
            {
                return Index == pattern.Length;
            }

            /// <summary>
            /// Whether the pattern at the current position matches the input.
            /// </summary>
            public bool Matches(char input, string pattern)
            {
                return pattern[Index] == input;
            }

            /// <summary>
            /// Produces a new match state which is one character furhter into
            /// the pattern.
            /// </summary>
            public FuzzyMatchState NextState()
            {
                return new FuzzyMatchState(Index + 1, Score);
            }
        }

        /// <summary>
        /// Scores how well a string is a fuzzy match for the pattern, with
        /// lower scores being better. null scores indicate no matches could be
        /// found.
        /// </summary>
        /// <remarks>
        /// You can thik of the pattern as being a translation of a regex,
        /// where "abcdef" is the pattern and ".*a.*b.*c.*e.*f.*" is the
        /// underlying regex. The score is then the number of characters which
        /// are consumed by the .* terms (or null if no match exists).
        /// </remarks>
        public static int? FuzzyMatch(string subject, string pattern)
        {
            subject = subject.ToLower();
            pattern = pattern.ToLower();

            // The reason we do it this way is because we want to get the best
            // score possible. For example, if we had the pattern "system" and the
            // input "sys_system", we want to choose the "system" as the fuzzy
            // match and not "sys____tem" (with _ as wildcards)
            //
            // In order to do that, we examine each possibility when there's a
            // match (either take it, or don't) and see which end state has the
            // best outcome
            var states = new List<FuzzyMatchState>();
            var completedStates = new List<FuzzyMatchState>();
            states.Add(new FuzzyMatchState(0, 0));

            foreach (var ch in subject)
            {
                var nextStates = new List<FuzzyMatchState>();
                foreach (var state in states)
                {
                    if (state.IsDone(pattern))
                    {
                        completedStates.Add(state);
                    }
                    else if (state.Matches(ch, pattern))
                    {
                        var nextState = state.NextState();
                        nextStates.Add(state);

                        if (nextState.IsDone(pattern))
                        {
                            completedStates.Add(nextState);
                        }
                        else
                        {
                            nextStates.Add(nextState);
                        }
                    }
                    else
                    {
                        // We only want the intervening characters to count, not
                        // anything that comes before the pattern entirely
                        if (state.Started())
                        {
                            state.Penalize();
                        }

                        nextStates.Add(state);
                    }
                }

                states = nextStates;
            }

            if (completedStates.Count == 0)
            {
                return null;
            }
            else
            {
                return completedStates.Min(state => state.Score);
            }
        }
    }
}
