﻿using StainedGlass.Persistence.Entities;
using StainedGlass.Persistence.Transfer;

namespace StainedGlass.Persistence.Services.DB;

internal class DbChurch : DatabasePersistenceService
{
    public override void AddEntity(IPersistanceTransferStruct transferStruct)
    {
        var itemStruct = (ChurchDTO)transferStruct;
        var churchEntity = new Church
        {
            Name = itemStruct.Name,
            Slug = itemStruct.Slug,
            Description = itemStruct.Description,
            Image = itemStruct.Image,
        };
        _dbContext.Churches.Add(churchEntity);
        _dbContext.SaveChanges();
    }

    public override IEnumerable<IPersistanceTransferStruct> GetAllDtos()
    {
        List<ChurchDTO> churchDtos = new();
        foreach (var churchEntity in _dbContext.Churches.ToList())
        {
            //todo: finish transformation into ChurchDTO
        }

        return churchDtos as IEnumerable<IPersistanceTransferStruct>;
    }
}