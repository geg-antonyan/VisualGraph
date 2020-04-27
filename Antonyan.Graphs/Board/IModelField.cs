﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board
{
    public interface IModelField
    {
        void CreateGraph(bool oriented, bool weighted, bool raise = true);
        void AddVertexModel(AVertexModel vertexModel, bool raise = true);
        void AddEdgeModel(AEdgeModel edgeModel, bool raise = true);
        void AddGraphModel(GraphModel model, bool raise = true);
        List<GraphModel> RemoveVertexModel(AVertexModel vertexModel, bool raise = true);
        GraphModel RemoveEdgeModel(AEdgeModel edgeModel, bool raise = true);
        List<GraphModel> RemoveGraphModel(GraphModel model, bool raise = true);
        List<GraphModel> RemoveGraphModels(List<GraphModel> models, bool raise = true);
        bool IsWeighted { get; }
        bool IsOrgraph { get; }
        UserInterface UserInterface { get; }
        void Clear(bool raise = true);

        void MoveVertexModel(string  key, vec2 newPos, bool raise = true);

        bool MarkGraphModel(string key, bool raise = true);
        void UnmarkGraphModels(bool raise = true);
        void Refresh();

        void SaveGraphToFile(Stream stream, bool raise = true);
        void OpenGraphInFile(List<GraphModel> models, string graphDataText, bool raise = true);
        string GetPosKey(vec2 pos, float r);
        string GetVertexPosKey(vec2 pos, float r);
        vec2 GetVertexModelPos(string key);
        List<GraphModel> Models { get; }
        GraphModel this[string key] { get; }
        List<GraphModel> GetMarkedModels();
        int MarkedModelsCount { get; }
        int MarkedVertexModelCount { get; }
        int MarkedEdgeModelCount { get; }
        bool Status { get; }


        // Tasks
        int GetHalfLifeDegree(AVertexModel vertex);

        // !Tasks
    }
}
