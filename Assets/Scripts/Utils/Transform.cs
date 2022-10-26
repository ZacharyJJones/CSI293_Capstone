using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Static Class containing all 'Transform' functions. Arguments named 't' should all be within the range of 0-1.
/// <para> The argument 't' is the variable value that you input into the equation, 't' being the interpolating value. </para>
/// </summary>
public static class Transform
{
  /// <summary> Returns the result of Sin(t), normalized such that one wave period occurs over an increment of 1. </summary>
  /// <param name="t"> Input value, used like interpolation, should be between 0-1. </param>
  /// <param name="xOffset"> X offset for sine wave. </param>
  /// <param name="magnitude"> The most extreme y-value deviation from the that this function will return. </param>
  /// <param name="midline"> The 'y-value' for the center between the peaks and troughs of the wave. </param>
  public static float Sine(float t, float xOffset = 0, float magnitude = 0.5f, float midline = 0.5f)
  {
    // 'sine' produces the result of Math.Sin, which (by default) is normalized to the 0-1 domain/range.
    // https://www.desmos.com/calculator/xp5uwiwywl

    // magnitude 0.5, center @ y=0.5, period = 1
    /* sample values:
    [0.00 ,   0.50],
    [0.25 ,     1.00],
    [0.50 ,   0.50],
    [0.75 , 0.00],
    [1.00 ,   0.50]
    */

    double ret = (Math.Sin((Math.PI * 2.0) * (t + xOffset)) * magnitude) + midline;
    return (float)ret;
  }

  /// <summary> Flips given 't' value. </summary>
  /// <param name="t"> Value to flip. </param>
  public static float Flip(float t) => 1.0f - t;

  /// <summary> Squares given 't' value. </summary>
  /// <param name="t"> Value to square. </param>
  public static float Square(float t) => t * t;

  /// <summary> Returns a mix of values 'a' and 'b', with the given mult applied to 'b', and (1 - mult) applied to 'a'. </summary>
  /// <param name="a"> The first input value. </param>
  /// <param name="b"> The second input value. </param>
  /// <param name="bMult"> The value to multiply 'b' by. Must be within the 0-1 range. </param>
  /// <returns></returns>
  public static float Mix(float a, float b, float bMult)
  {
    if (bMult > 1.0f) bMult = 1.0f;
    else if (bMult < 0.0f) bMult = 0.0f;

    return ((1.0f - bMult) * a) + (bMult * b);
  }

  /// <summary> Returns a value which is a mix of [t] between [a] and [b].  </summary>
  /// <param name="a"> The value to return when 't' equals 0. </param>
  /// <param name="b"> The value to return when 't' equals 1. </param>
  /// <param name="t"> Value to fade from 'a' to 'b' with. </param>
  public static float Crossfade(float a, float b, float t) => Mix(a, b, t);

  /// <summary> Multiplies 't' by the given value 'a'. </summary>
  /// <param name="t"> The value to multiply by 'a'. </param>
  /// <param name="a"> The value to multiply 't' by. </param>
  public static float Scale(float t, float a) => t * a;


  /// <summary> Multiplies value 't' by itself 'x' times. </summary>
  /// <param name="t"> The value to be raised to a power. </param>
  /// <param name="x"> The power to raise 't' to. </param>
  /// <returns></returns>
  public static float Power(float t, int x)
  {
    if (x <= 0) return 1;

    float tRet = t;
    while (x > 1)
    {
      tRet *= t;
      x--;
    }

    return tRet;
  }

  /// <summary> Returns the absolute value of the given float. </summary>
  /// <param name="a"> Float to get the absolute value of. </param>
  public static float Abs(float a) => (a >= 0) ? a : -a;


  /// <summary> If 't' is less than 'x', 't' is made equal to 'x'. </summary>
  /// <param name="t"> Value to not allow to be less than 'x'. </param>
  /// <param name="x"> Value which 't' is not allowed to be less than. </param>
  public static float Min(float t, float x) => (t < x) ? x : t;

  /// <summary> If 't' is greater than 'x', 't' is made equal to 'x' </summary>
  /// <param name="t"> Value to not allow to be greater than 'x'. </param>
  /// <param name="x"> Value which 't' is not allowed to be greater than. </param>
  public static float Max(float t, float x) => (t > x) ? x : t;

  /// <summary> Uses a known linear equation to get a value. Returns 'f(x) = b + m*x' </summary>
  /// <param name="x"> The 'x-value' to input into the equation. </param>
  /// <param name="m"> The 'slope' of the equation. </param>
  /// <param name="b"> The 'y-intercept' of the equation. </param>
  public static float LinEq(float x, float m, float b) => (m * x) + b;

  /// <summary> Returns an interpolated value between 'min' and 'max' </summary>
  /// <param name="t"> Value to interpolate with. </param>
  /// <param name="min"> Value to return when 't' equals 0. </param>
  /// <param name="max"> Value to return when 't' equals 1. </param>
  public static float Lerp(float t, float min, float max) => min + ((max - min) * t);

  /// <summary> Returns a value [0..1] which represents the fraction between [min] and [max] that [a] is. </summary>
  /// <param name="a"> Value between [min] and [max]. Note: if [a]&gt;[max], ret will be &gt;[1]. If [a]&lt;[min], ret will be &lt;[0]. </param>
  /// <param name="min"> The minimum expected value for [a] to be. </param>
  /// <param name="max"> The maximum expected value for [a] to be. </param>
  public static float InverseLinInterpolate(float a, float min, float max) => (a - min) / (max - min);

  private static float _floatPowCommon(float t, float pow, Func<float, int, float> func)
  {
    var bPow = new BetterPow(pow);
    return Mix(func(t, bPow.PowWhole), func(t, bPow.PowWhole + 1), bPow.PowDecimal);
  }

  /// <summary> Returns a value which increases slowly at low values of 't', and more rapidly as 't' approaches 1. </summary>
  /// <param name="t"> Value to apply 'SmoothStart' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStartX(float t, float pow) => _floatPowCommon(t, pow, SmoothStartX);

  /// <summary> Returns a value which increases slowly at low values of 't', and more rapidly as 't' approaches 1. </summary>
  /// <param name="t"> Value to apply 'SmoothStart' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStartX(float t, int pow) => Power(t, pow);

  /// <summary> Returns a value which increases rapidly at low values of 't', but more slowly as 't' approaches 1. </summary>
  /// <param name="t"> Value to apply 'SmoothStop' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStopX(float t, float pow) => _floatPowCommon(t, pow, SmoothStopX);

  /// <summary> Returns a value which increases rapidly at low values of 't', but more slowly as 't' approaches 1. </summary>
  /// <param name="t"> Value to apply 'SmoothStop' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStopX(float t, int pow) => Flip(Power(Flip(t), pow));

  /// <summary> Returns a value which increases slowly when 't' is near 0 and 1, but increases rapidly when 't' is near 0.5. </summary>
  /// <param name="t"> Value to apply 'SmoothStep' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStepX(float t, float pow) => _floatPowCommon(t, pow, SmoothStepX);

  /// <summary> Returns a value which increases slowly when 't' is near 0 and 1, but increases rapidly when 't' is near 0.5. </summary>
  /// <param name="t"> Value to apply 'SmoothStep' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float SmoothStepX(float t, int pow) => Crossfade(SmoothStartX(t, pow), SmoothStopX(t, pow), t);

  /// <summary> Returns a value which equals 0 when [t] equals 0 or 1, but returns 1 when [t] equals 0.5. </summary>
  /// <param name="t"> Value to apply 'Arch' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float ArchX(float t, float pow) => _floatPowCommon(t, pow, ArchX);

  /// <summary> Returns a value which equals 0 when [t] equals 0 or 1, but returns 1 when [t] equals 0.5. </summary>
  /// <param name="t"> Value to apply 'Arch' function to. </param>
  /// <param name="pow"> Higher values for this parameter result in more dramatic curves. </param>
  public static float ArchX(float t, int pow) => Power(4.0f * t * Flip(t), pow);

  /// <summary> Creates a linear equation intercepting the points (x,y) and (1,1) and plugs in 't' to get the result. </summary>
  /// <param name="t"> The value [0..1] to be plugged into the calculated equation. </param>
  /// <param name="p"> The (x,y) point which this equation passes through. </param>
  public static float LinEqGivenPoint_ToOne(float t, (float X, float Y) p) => LinEqGivenPoints(t, p, (1, 1));

  /// <summary> Creates a linear equation intercepting the points (0,0) and (x,y) and plugs in 't' to get the result. </summary>
  /// <param name="t"> The interpolant value to use between the point (0,0) and the given point. </param>
  /// <param name="p"> The other point to use when calculating the line from (0,0). </param>
  public static float LinEqGivenPoint_FromZero(float t, (float X, float Y) p) => LinEqGivenPoints(t, (0, 0), p);


  /// <summary> Creates a linear equation intercepting the points (x1,y1) and (x2,y2) and plugs in 't' to get the result. </summary>
  /// <param name="t"> Value to plug into equation as 'x'. </param>
  /// <param name="pointA"> One of the two points which the equation passes through. </param>
  /// <param name="pointB"> The other of the two points which the equation passes through. </param>
  /// <returns></returns>
  public static float LinEqGivenPoints(float t, (float X, float Y) pointA, (float X, float Y) pointB)
  {
    // slope is (y2-y1)/(x2-x1)
    // y-intercept is (y - m*x) (with y and x in this case being coordinates of one of the points)
    // so equation to get Y at point T given line between x,y and 1,1 is
    // ret =       m   *  (t) +            b
    // ret = ((y2-y1)/(x2-x1))(t) + (y2 - ((y2-y1)/(x2-x1))*x2)

    float m = (pointB.Y - pointA.Y) / (pointB.X - pointA.X);
    float b = (pointA.Y - (m * pointA.X));
    return LinEq(t, m, b);
  }


  /// <summary> Returns a specific row of the Pascal triangle. </summary>
  /// <param name="rowIndex"> The zero-based index for the pascal row to get. 0={1}, 1={1,1}, 2={1,2,1}, etc.</param>
  public static List<int> GetPascalRow(int rowIndex)
  {
    // rowIndex 0 (1) = {  1  }
    // rowIndex 1 (2) = {  1  ,  1  }
    // rowIndex 2 (3) = {  1  ,  2  ,  1  }
    // rowIndex 3 (4) = {  1  ,  3  ,  3  ,  1  }
    // rowIndex 4 (5) = {  1  ,  4  ,  6  ,  4  ,   1 }
    // rowIndex 5 (6) = {  1  ,  5  , 10  , 10  ,   5 ,   1 }
    // rowIndex 6 (7) = {  1  ,  6  , 15  , 20  ,  15 ,   6 ,  1  }
    // rowIndex 7 (8) = {  1  ,  7  , 21  , 35  ,  35 ,  21 ,  7  ,  1  }
    // rowIndex 8 (9) = {  1  ,  8  , 28  , 56  ,  70 ,  56 , 28  ,  8  ,  1  }
    // rowIndex 9 (10)= {  1  ,  9  , 36  , 84  , 126 , 126 , 84  , 36  ,  9  ,  1  }
    //================================================================================

    if (rowIndex < 0) return new List<int>();
    if (rowIndex == 0) return new List<int> { 1 };

    var prevRow = GetPascalRow(rowIndex - 1);

    var retRow = new List<int>(rowIndex + 1) { [0] = 1, [rowIndex] = 1 };
    for (var i = 1; i < rowIndex; i++)
    {
      retRow.Add(prevRow[i] + prevRow[i - 1]);
    }

    return retRow;
  }


  /// <summary>
  /// <para> Returns the corresponding value of the normalized Bezier curve supplied via [vals] at x=[t]. </para>
  /// <para> This method returns a 'normalized' value, meaning that when [t]=0, method returns 0, and when [t]=1, returns 1. </para>
  /// </summary>
  /// <param name="t"> Value to use as 'x' to get corresponding Bezier curve value. </param>
  /// <param name="mids"> Collection of evenly spaced points between [0,1], exclusive, to form the Bezier curve from. </param>
  public static float NormalizedBezierX(float t, IEnumerable<float> mids)
  {
    #region example data:

    #region mids.count = 0

    // (midsfull makes everything line up better)
    // mids =     {...... ......}
    // midsfull = { [0]0 , [1]1 }
    // pascal   = { [0]1 , [1]1 }

    // numMidsFull == 2 == numPascal

    // ex: if (i == 1)
    // ret += pas[i] * midsFull[i] * Power(s, (numMidsFull - 1) - i) * Power(t, i)
    // ret += pas[1] * midsFull[1] * Power(s, (2 - 1) - 1)           * Power(t, 1)
    // ret += 1      * 1           * s^0                             * t^1


    // ret += 1 * 0 * 1 * 1 ( when i == 0)
    // ret += 1 * 1 * 1 * 1 ( when i == 1)

    // result is linear

    // full breakdown follows

    // i = 0 | 1  * 0  * 0^0 * 1^1
    // i = 1 | 1  * 1  * 1^1 * 0^0

    #endregion

    #region mids.count = 4

    // (midsfull makes everything line up better)
    // mids =     {......  [0]0.5 , [1]1.0 , [2]0.7 , [3]0.2  ......}
    // midsfull = { [0]0 , [1]0.5 , [2]1.0 , [3]0.7 , [4]0.2 , [5]1 }
    // pascal   = { [0]1 , [1]5   , [2]10  , [3]10  , [4]5   , [5]1 }

    // numMidsFull == 6 == numPascal

    // ex: if (i == 3)
    // ret += pas[i] * midsFull[i] * Power(s, (numMidsFull - 1) - i) * Power(t, i)
    // ret += pas[3] * midsFull[3] * Power(s, (6 - 1) - 3)           * Power(t, 3)
    // ret += 10     * 0.7         * s^2                             * t^3

    // full breakdown follows

    // i = 0 | 1  * 0   * s^5 * t^0
    // i = 1 | 5  * 0.5 * s^4 * t^1
    // i = 2 | 10 * 1.0 * s^3 * t^2
    // i = 3 | 10 * 0.7 * s^2 * t^3
    // i = 4 | 5  * 0.2 * s^1 * t^4
    // i = 5 | 1  * 1   * s^0 * t^5

    #endregion

    #endregion

    // init 'midsfull' for easier programming
    var allVals = new List<float> { 0 };
    allVals.AddRange(mids);
    allVals.Add(1);

    return BezierX(t, allVals);
  }

  /// <summary> Returns the corresponding value of the Bezier curve supplied via [vals] at x=[t]. </summary>
  /// <param name="t"> Value to use as 'x' to get corresponding Bezier curve value. </param>
  /// <param name="vals"> Collection of values spaced evenly between [0,1] (inclusive) which the Bezier curve will be formed from. </param>
  public static float BezierX(float t, IEnumerable<float> vals)
  {
    var valsList = vals.ToList();
    int count = valsList.Count;

    if (count == 1) return valsList.ElementAt(0);
    if (count <= 2) return Lerp(t, 0, 1);

    var pascal = GetPascalRow(count - 1);

    return valsList.Select(
        (t1, i) => pascal[i] * t1 * Power((1 - t), (count - 1) - i) * Power(t, i)
    ).Sum();
  }
}

internal readonly struct BetterPow
{
  public BetterPow(float startPow)
  {
    PowWhole = (int)Math.Floor(startPow);
    PowDecimal = startPow - PowWhole;
  }

  public readonly int PowWhole;
  public readonly float PowDecimal;
}
