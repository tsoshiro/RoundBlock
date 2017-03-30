/// <summary>
/// レイヤー名を定数で管理するクラス
/// </summary>
public static class LayerName
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int Ball = 8;
	public const int Block = 9;
	public const int HardBall = 10;
	public const int Racket = 11;
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int BallMask = 256;
	public const int BlockMask = 512;
	public const int HardBallMask = 1024;
	public const int RacketMask = 2048;
}
