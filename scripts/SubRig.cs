using Godot;
using System.Collections.Generic;

[Tool]
public partial class SubRig : Node3D
{
    [Export] public float MinInfluence { get; set; } = 0.5f;
    [Export] public float MaxInfluence { get; set; } = 1.0f;

    [Export]
    public float LoopDuration { get; set; } = 1.0f;

    [Export] public Skeleton3D Master { get; set; }

    private Skeleton3D localSkeleton;
    private List<SpringBoneSimulator3D> springBoneSimulators = new List<SpringBoneSimulator3D>();

    private bool increasing = true;
    private float currentInfluence;
    private float influenceStep;
    private const float speedMultiplier = 10f; // Matches multiplier in _Process

    public override void _Ready()
    {
        SetLocalRig();
        FindAllSpringBoneSimulators(this);

        currentInfluence = MinInfluence;

        // Calculate influenceStep based on LoopDuration and range
        float range = MaxInfluence - MinInfluence;
        if (LoopDuration <= 0)
        {
            GD.PrintErr("LoopDuration must be greater than zero. Setting to 5 seconds.");
            LoopDuration = 5f;
        }
        influenceStep = (2f * range) / (speedMultiplier * LoopDuration);

        GD.Print($"Found {springBoneSimulators.Count} SpringBoneSimulator3D nodes.");
        GD.Print($"Calculated InfluenceStep = {influenceStep} based on LoopDuration = {LoopDuration}s");
    }

    private void FindAllSpringBoneSimulators(Node node)
    {
        if (node is SpringBoneSimulator3D spring)
        {
            springBoneSimulators.Add(spring);
        }

        foreach (Node child in node.GetChildren())
        {
            FindAllSpringBoneSimulators(child);
        }
    }

    public override void _Process(double delta)
    {
        CopyMasterRig();
        CopyMasterRigFollowUp(delta);
    }

    private void SetLocalRig()
    {
        localSkeleton = GetNodeOrNull<Skeleton3D>("%GeneralSkeleton");

        if (localSkeleton == null)
        {
            GD.PrintErr("SubRig could not find a Skeleton3D named 'GeneralSkeleton'.");
        }
    }

    private void CopyMasterRig()
    {
        if (Master == null || localSkeleton == null)
            return;

        int boneCount = Master.GetBoneCount();
        for (int i = 0; i < boneCount; i++)
        {
            string boneName = Master.GetBoneName(i);
            int subBoneIndex = localSkeleton.FindBone(boneName);

            if (subBoneIndex == -1)
                continue;

            Transform3D masterGlobalTransform = Master.GetBoneGlobalPose(i);

            int parentIndex = localSkeleton.GetBoneParent(subBoneIndex);

            Transform3D localTransform;

            if (parentIndex == -1)
            {
                localTransform = masterGlobalTransform;
            }
            else
            {
                Transform3D parentGlobalTransform = localSkeleton.GetBoneGlobalPose(parentIndex);
                localTransform = parentGlobalTransform.AffineInverse() * masterGlobalTransform;
            }

            localSkeleton.SetBonePose(subBoneIndex, localTransform);
        }
    }

    private void CopyMasterRigFollowUp(double delta)
    {
        if (springBoneSimulators.Count == 0)
            return;

        // Update currentInfluence with frame-rate independent step
        if (increasing)
        {
            currentInfluence += influenceStep * (float)delta * speedMultiplier;
            if (currentInfluence >= MaxInfluence)
            {
                currentInfluence = MaxInfluence;
                increasing = false;
            }
        }
        else
        {
            currentInfluence -= influenceStep * (float)delta * speedMultiplier;
            if (currentInfluence <= MinInfluence)
            {
                currentInfluence = MinInfluence;
                increasing = true;
            }
        }

        // Apply influence to all spring bone simulators
        foreach (var spring in springBoneSimulators)
        {
            spring.Influence = currentInfluence;
        }
    }
}
