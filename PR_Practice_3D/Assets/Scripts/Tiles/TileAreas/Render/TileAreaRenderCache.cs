using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRTileArea.Rendering {
	public class TileAreaRenderCache {

		private List<RenderCache> renderCaches;

		private class RenderCache {
			public Mesh mesh;
			public Material material;
			public Matrix4x4[] matrices;

			public RenderCache(Mesh mesh, Material material, Matrix4x4[] matrices) {
				this.mesh = mesh;
				this.material = material;
				this.matrices = matrices;
			}

		}

		public TileAreaRenderCache() {
			renderCaches = new List<RenderCache>();
		}

		public void ClearCache() {
			renderCaches.Clear();
		}

		public void AddCache(Mesh mesh, Material material, Matrix4x4[] matrices) {
			renderCaches.Add(new RenderCache(mesh, material, matrices));
		}

		public void Render() {
			foreach (RenderCache cache in renderCaches) {
				Graphics.DrawMeshInstanced(cache.mesh, 0, cache.material, cache.matrices);
			}
		}

	}
}
