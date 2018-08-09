/// Credit Alastair Aitchison
/// Sourced from - https://bitbucket.org/UnityUIExtensions/unity-ui-extensions/issues/123/uilinerenderer-issues-with-specifying

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/UI Line Connector")]
    [RequireComponent(typeof(UILineRenderer))]
    [ExecuteInEditMode]
    public class UILineConnector : MonoBehaviour
    {

        // The elements between which line segments should be drawn
        public RectTransform[] transforms;
        private Vector2[] previousPositions;
        private RectTransform canvas;
        private RectTransform rt;
        private UILineRenderer lr;

        private void Awake()
        {
            canvas = GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
            rt = GetComponent<RectTransform>();
            lr = GetComponent<UILineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transforms == null || transforms.Length < 1)
            {
                return;
            }
            //Performance check to only redraw when the child transforms move
            if (previousPositions != null && previousPositions.Length == transforms.Length)
            {
                bool updateLine = false;
                for (int i = 0; i < transforms.Length; i++)
                {
                    if (!updateLine && previousPositions[i] != transforms[i].anchoredPosition)
                    {
                        updateLine = true;
                    }
                }
                if (!updateLine) return;
            }

            // Get the pivot points
            Vector2 thisPivot = rt.pivot;
            Vector2 canvasPivot = canvas.pivot;

            // Set up some arrays of coordinates in various reference systems
            Vector3[] worldSpaces = new Vector3[transforms.Length];
            Vector3[] canvasSpaces = new Vector3[transforms.Length];
            Vector2[] points = new Vector2[transforms.Length];

            // First, convert the pivot to worldspace
            for (int i = 0; i < transforms.Length; i++)
            {
                worldSpaces[i] = transforms[i].TransformPoint(thisPivot);
            }

            // Then, convert to canvas space
            for (int i = 0; i < transforms.Length; i++)
            {
                canvasSpaces[i] = canvas.InverseTransformPoint(worldSpaces[i]);
            }

            // Calculate delta from the canvas pivot point
            for (int i = 0; i < transforms.Length; i++)
            {
                points[i] = new Vector2(canvasSpaces[i].x, canvasSpaces[i].y);
            }

			Vector2[] curvePoints = new Vector2[1 + ((transforms.Length - 1) *3)];

			int nextCurvePoint = 0;

			for (int i = 0; i < points.Length - 1; i++) {

				Vector2 start = points [i];
				Vector2 end = points [i + 1];

				curvePoints [nextCurvePoint] = start;

				nextCurvePoint++;

				curvePoints[nextCurvePoint] = new Vector2(start.x, start.y + 50f);

				nextCurvePoint++;

				curvePoints[nextCurvePoint] = new Vector2((end.x - start.x)/2 + start.x, Mathf.Max(start.y + 50f, end.y));

				nextCurvePoint++;

			
				if (i == points.Length - 2) {
					curvePoints [nextCurvePoint] = end;
				}

					
			}	



            // And assign the converted points to the line renderer
            lr.Points = curvePoints;
            lr.RelativeSize = false;
            lr.drivenExternally = true;

            previousPositions = new Vector2[transforms.Length];
            for (int i = 0; i < transforms.Length; i++)
            {
                previousPositions[i] = transforms[i].anchoredPosition;
            }
        }
    }
}