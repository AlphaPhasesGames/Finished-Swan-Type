using UnityEngine;

public interface IPaintCoverage
{
    float CoveragePercent { get; }
    bool IsComplete { get; }
}