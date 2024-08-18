namespace RapidNet.Extensions
{
    /// <summary>
    /// Enum used to tag  what thread is sending data.
    /// </summary>
    public enum ThreadType
    {
        /// <summary>
        /// The game thread.
        /// </summary>
        Game,
        /// <summary>
        /// The logic thread.
        /// </summary>
        Logic,

        /// <summary>
        /// The network thread.
        /// </summary>
        Network
    }
}
