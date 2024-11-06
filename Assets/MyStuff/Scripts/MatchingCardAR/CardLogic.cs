using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public string plantName = "DefaultCardName";
    public int id = 0;
    public Color col = Color.white;
    public bool isImage = false;
    public string description;

    [SerializeField] GameObject cardTextFront;
    [SerializeField] GameObject cardTextBack;
    [SerializeField] GameObject imgFront;
    [SerializeField] GameObject imgBack;
    private Transform highlight;

    public void SetCard(MatchingCardSO cardData, bool isImage)
    {
        this.plantName = cardData.name;
        this.id = cardData.id;
        this.isImage = isImage;
        this.description = cardData.getText();
        if (isImage)
        {
            showImage(cardData);
        } else
        {
            showText(cardData);
        }
    }

    private void showImage(MatchingCardSO cardData)
    {
        imgFront.SetActive(true);
        imgBack.SetActive(true);
        cardTextFront.SetActive(false);
        cardTextBack.SetActive(false);
        imgFront.GetComponent<Image>().sprite = cardData.img;
        imgBack.GetComponent<Image>().sprite = cardData.img;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            Debug.Log("Set");
            meshRenderer.material.color = cardData.col;
        }
    }

    private void showText(MatchingCardSO cardData)
    {
        imgFront.SetActive(false);
        imgBack.SetActive(false);
        cardTextFront.SetActive(true);
        cardTextBack.SetActive(true);
        cardTextFront.GetComponent<TextMeshProUGUI>().text = description;
        cardTextBack.GetComponent<TextMeshProUGUI>().text = description;
    }

    public void Select()
    {
        if (highlight.gameObject.GetComponent<Outline>() != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = true;
        } else
        {
            Outline outline = highlight.gameObject.AddComponent<Outline>();
            outline.enabled = true;
            highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
            highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
        }
    }

    public bool IsMatching(CardLogic otherCard)
    {
        return otherCard.id == id & otherCard != this;
    }

    public void Deselect()
    {
        highlight.gameObject.GetComponent<Outline>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        highlight = this.transform;
    }
}
