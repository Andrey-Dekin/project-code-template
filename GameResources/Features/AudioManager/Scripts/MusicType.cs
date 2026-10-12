namespace WinterDrift.Features.AudioManager
{
    /// <summary>
    /// Тип музыки для воспроизведения в разных состояниях игры
    /// </summary>
    public enum MusicType
    {
        /// <summary>
        /// Музыка не воспроизводится
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Музыка для меню и пользовательского интерфейса
        /// </summary>
        Menu = 1,
        
        /// <summary>
        /// Музыка для игрового процесса
        /// </summary>
        Gameplay = 2
    }
}