﻿using StainedGlass.Entities;

namespace StainedGlass.Transfer.Mapper;

public interface NonRelatable : Mappable
{
    public Transferable? GetDTO(Entity? entity, bool skipParentElements = false, bool skipChildrenElements = false);
}