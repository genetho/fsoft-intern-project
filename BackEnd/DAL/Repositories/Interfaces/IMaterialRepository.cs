﻿using DAL.Entities;

namespace DAL.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        List<Material> GetLessonMaterials(long? lessonId);
        void Update(Material material);
        Material GetById(long id);
        Material GetByFullId(long id);
        Material Create(Material material);
        // Team6
        void DeleteMaterial(long? id);
        // Team 01
        void Deactivate(long id);
        void Activate(long id);
    }
}
