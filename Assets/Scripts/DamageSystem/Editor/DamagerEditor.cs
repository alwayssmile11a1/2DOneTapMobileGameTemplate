using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Gamekit2D
{
    [CustomEditor(typeof(Damager))]
    public class DamagerEditor : Editor
    {
        static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
        static Color s_EnabledColor = Color.green + Color.grey;

        SerializedProperty m_UseTriggerColliderProp;
        SerializedProperty m_OffsetProp;
        SerializedProperty m_SizeProp;
        SerializedProperty m_OffsetBasedOnSpriteFacingProp;
        SerializedProperty m_SpriteRendererProp;
        SerializedProperty m_EnableDamageOnAwake;
        SerializedProperty m_CanHitTriggersProp;
        SerializedProperty m_DisableDamageAfterHit;
        SerializedProperty m_ReceiveOnHitEventOnInvincibleObject;
        SerializedProperty m_IgnoreInvincibilityProp;
        SerializedProperty m_HittableLayersProp;
        SerializedProperty m_OnDamageableHitProp;
        SerializedProperty m_OnNonDamageableHitProp;

        void OnEnable ()
        {
            m_UseTriggerColliderProp = serializedObject.FindProperty("useTriggerCollider");
            m_OffsetProp = serializedObject.FindProperty("offset");
            m_SizeProp = serializedObject.FindProperty("size");
            m_OffsetBasedOnSpriteFacingProp = serializedObject.FindProperty("offsetBasedOnSpriteFacing");
            m_SpriteRendererProp = serializedObject.FindProperty("spriteRenderer");
            m_EnableDamageOnAwake = serializedObject.FindProperty("enableDamageOnAwake");
            m_CanHitTriggersProp = serializedObject.FindProperty("canHitTriggers");
            m_DisableDamageAfterHit = serializedObject.FindProperty("disableDamageAfterHit");
            m_ReceiveOnHitEventOnInvincibleObject = serializedObject.FindProperty("receiveOnHitEventOnInvincibleObject");
            m_IgnoreInvincibilityProp = serializedObject.FindProperty("ignoreInvincibility");
            m_HittableLayersProp = serializedObject.FindProperty("hittableLayers");
            m_OnDamageableHitProp = serializedObject.FindProperty("OnDamageableHit");
            m_OnNonDamageableHitProp = serializedObject.FindProperty("OnNonDamageableHit");
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update ();

            EditorGUILayout.PropertyField(m_UseTriggerColliderProp);
            if (!m_UseTriggerColliderProp.boolValue)
            {
                EditorGUILayout.PropertyField(m_OffsetProp);
                EditorGUILayout.PropertyField(m_SizeProp);
                EditorGUILayout.PropertyField(m_OffsetBasedOnSpriteFacingProp);
                if (m_OffsetBasedOnSpriteFacingProp.boolValue)
                    EditorGUILayout.PropertyField(m_SpriteRendererProp);
                EditorGUILayout.PropertyField(m_CanHitTriggersProp);
            }
            EditorGUILayout.PropertyField(m_EnableDamageOnAwake);
            EditorGUILayout.PropertyField(m_DisableDamageAfterHit);
            EditorGUILayout.PropertyField(m_ReceiveOnHitEventOnInvincibleObject);
            EditorGUILayout.PropertyField(m_IgnoreInvincibilityProp);
            EditorGUILayout.PropertyField(m_HittableLayersProp);
            EditorGUILayout.PropertyField(m_OnDamageableHitProp);
            EditorGUILayout.PropertyField(m_OnNonDamageableHitProp);

            serializedObject.ApplyModifiedProperties ();
        }

        void OnSceneGUI ()
        {
            Damager damager = (Damager)target;

            if (!damager.enabled || m_UseTriggerColliderProp.boolValue)
                return;

            Matrix4x4 handleMatrix = damager.transform.localToWorldMatrix;
            handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
            handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
            handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, damager.transform.position.z));
            using (new Handles.DrawingScope(handleMatrix))
            {
                s_BoxBoundsHandle.center = damager.offset;
                s_BoxBoundsHandle.size = damager.size;

                s_BoxBoundsHandle.SetColor(s_EnabledColor);

                //Check if any control was changed inside a block of code.
                EditorGUI.BeginChangeCheck();

                // Block of code with controls
                // this may set GUI.changed to true.
                s_BoxBoundsHandle.DrawHandle();

                //if true, GUI.changed was set to true
                if (EditorGUI.EndChangeCheck())
                {
                    // Code to execute if GUI.changed
                    // was set to true inside the block of code above.

                    //Records any changes done on the object after the RecordObject function
                    //name: The title of the action to appear in the undo history (i.e. visible in the undo menu).
                    Undo.RecordObject(damager, "Modify Damager");

                    damager.size = s_BoxBoundsHandle.size;
                    damager.offset = s_BoxBoundsHandle.center;
                }
            }
        }
    }
}