using System.Collections.Generic;
using uFrame.Editor.Koinonia.Classes;
using uFrame.Editor.Koinonia.Data;

namespace uFrame.Editor.Koinonia.Service
{
    public interface IDesctiptorsService
    {
        UFramePackageDescriptor GetPackageDescriptorById(string id);
        UFramePackageDescriptor GetPackageDescriptorByRevision(UFramePackageRevisionDescriptor revision);
        UFramePackageDescriptor GetPackageDescriptorByPackage(UFramePackage revision);

        UFramePackageRevisionDescriptor GetRevisionById(string id);
        UFramePackageRevisionDescriptor GetRevisionDescriptorByPackageIdAndTag(string packageId, string tag);
        IEnumerable<UFramePackageRevisionDescriptor> GetRevisionsByProject(UFramePackageDescriptor package);

        IEnumerable<UFramePackageDescriptor> GetLatest();
        IEnumerable<UFramePackageDescriptor> Search();

    }
}