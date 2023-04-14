using Cinemachine;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : MonoBehaviour
{
    [Header("Fire")]
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform firePoint;

    [Header("Scope")]
    [SerializeField]
    CinemachineVirtualCamera scopeCamera;
    [SerializeField]
    LayerMask scopeLayerMask;

    [Header("First Person Controller")]
    [SerializeField]
    FirstPersonController firstPersonController;

    [SerializeField]
    StarterAssetsInputs input;

    [SerializeField]
    float normalSensitivity = 1.0f;

    [SerializeField]
    float scopeSenistivity = 0.5f;
    Animator animator;
    bool isScope;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        HandleShoot();
        HandleScope();
    }

    private void HandleScope()
    {
        if(isScope != input.scope)
        {
            isScope = input.scope;
            animator.SetBool("isScope", isScope);

            scopeCamera.gameObject.SetActive(isScope);
            firstPersonController.SetSensitivity(isScope ? scopeSenistivity : normalSensitivity);
        }
    }

    private void HandleShoot()
    {
        if (input.shoot)
        {
            Shoot();
            input.shoot = false;
        }
    }

    void Shoot()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint =
            new Vector2(Screen.width / 2.0F, Screen.height / 2.0F);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999.9F, scopeLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        Vector3 shootDirection =
            (mouseWorldPosition - firePoint.position).normalized;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection, Vector3.up));
    }
}
