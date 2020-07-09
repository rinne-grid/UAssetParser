using System;

namespace UAssetParser.Enums
{
	[Flags]
	public enum EObjectFlags : uint
	{
		NoFlags = 0x0u,
		Public = 0x1u,
		Standalone = 0x2u,
		MarkAsNative = 0x4u,
		Transactional = 0x8u,
		ClassDefaultObject = 0x10u,
		ArchetypeObject = 0x20u,
		Transient = 0x40u,
		MarkAsRootSet = 0x80u,
		TagGarbageTemp = 0x100u,
		NeedInitialization = 0x200u,
		NeedLoad = 0x400u,
		KeepForCooker = 0x800u,
		NeedPostLoad = 0x1000u,
		NeedPostLoadSubobjects = 0x2000u,
		NewerVersionExists = 0x4000u,
		BeginDestroyed = 0x8000u,
		FinishDestroyed = 0x10000u,
		BeingRegenerated = 0x20000u,
		DefaultSubObject = 0x40000u,
		WasLoaded = 0x80000u,
		TextExportTransient = 0x100000u,
		LoadCompleted = 0x200000u,
		InheritableComponentTemplate = 0x400000u,
		DuplicateTransient = 0x800000u,
		StrongRefOnFrame = 0x1000000u,
		NonPIEDuplicateTransient = 0x2000000u,
		Dynamic = 0x4000000u,
		WillBeLoaded = 0x8000000u
	}
}
