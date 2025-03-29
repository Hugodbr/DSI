using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; 


namespace Lab6_namespace
{
    public class Lab6 : MonoBehaviour
    {
        VisualElement botonCrear;
        VisualElement botonGuardar;
        Toggle toggleModificar;
        VisualElement contenedor_dcha;
        TextField input_nombre;
        TextField input_apellido;
        VisualElement input_imagen;
        Individuo individuoSelec;
        StyleBackground imagenSelec;
        List<VisualElement> imagenes;

        List<Individuo> listIndividuos = new List<Individuo>();

        private void OnEnable()
        {




            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            contenedor_dcha = root.Q<VisualElement>("dcha");
            input_nombre = root.Q<TextField>("inputNombre");
            input_apellido = root.Q<TextField>("inputApellido");
            botonCrear = root.Q<Button>("crear");
            botonGuardar = root.Q<Button>("guardar");
            toggleModificar = root.Q<Toggle>("modificar");
            imagenes = root.Q<VisualElement>("Contenedor").Children().ToList();
            LoadImages(ref imagenes);

            imagenSelec = imagenes.First().style.backgroundImage;

            botonCrear.RegisterCallback<ClickEvent>(NuevaTarjeta);
            botonGuardar.RegisterCallback<ClickEvent>(SaveIndividuosToFile);
            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);
            contenedor_dcha.RegisterCallback<ClickEvent>(SeleccionTarjeta);
            imagenes.ForEach(caja => {
                caja.RegisterCallback<ClickEvent>(SeleccionImagen);
                Debug.Log(caja);
                });
        }

        void SeleccionTarjeta(ClickEvent e)
        {
            VisualElement mTarjeta = e.target as VisualElement;
            individuoSelec = mTarjeta.userData as Individuo;

            if (individuoSelec != null)
            {
                input_nombre.SetValueWithoutNotify(individuoSelec.Nombre);
                input_apellido.SetValueWithoutNotify(individuoSelec.Apellido);
                toggleModificar.value = true;

                TarjetasBordeNegro();
                TarjetasBordeBlanco(mTarjeta);
            }
        }

        void NuevaTarjeta(ClickEvent evt)
        {
            if (!toggleModificar.value)
            {
                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("Tarjeta");
                VisualElement tarjetaPlantilla = plantilla.Instantiate();

                contenedor_dcha.Add(tarjetaPlantilla);
                TarjetasBordeNegro();
                TarjetasBordeBlanco(tarjetaPlantilla);

                Individuo individuo = new Individuo(
                    input_nombre.value, 
                    input_apellido.value, 
                    imagenSelec
                );
                Tarjeta tarjeta = new Tarjeta(tarjetaPlantilla, individuo);
                individuoSelec = individuo;

                listIndividuos.Add(individuo);
                string listaToJson = JsonHelper.ToJson(listIndividuos, true);
                Debug.Log(listaToJson);
                Debug.Log(Application.persistentDataPath);
            }
        }

        void CambioNombre(ChangeEvent<string> evt){
            if (toggleModificar.value){
                individuoSelec.Nombre = evt.newValue;
            }
        }

        void CambioApellido(ChangeEvent<string> evt){
            if (toggleModificar.value){
                individuoSelec.Apellido = evt.newValue;
            }
        }

        void TarjetasBordeNegro()
        {
            List<VisualElement> listaTarjetas = contenedor_dcha.Children().ToList();
            listaTarjetas.ForEach(elem =>
                {
                    VisualElement tarjeta = elem.Q("Tarjeta");

                    tarjeta.style.borderBottomColor = Color.black;
                    tarjeta.style.borderRightColor = Color.black;
                    tarjeta.style.borderTopColor = Color.black;
                    tarjeta.style.borderLeftColor = Color.black;
                });
        }

        void TarjetasBordeBlanco(VisualElement tar)
        {
            VisualElement tarjeta = tar.Q("Tarjeta");

            tarjeta.style.borderBottomColor = Color.white;
            tarjeta.style.borderRightColor = Color.white;
            tarjeta.style.borderTopColor = Color.white;
            tarjeta.style.borderLeftColor = Color.white;
        }

        void SaveIndividuosToFile(ClickEvent evt)
        {
            string json = JsonHelper.ToJson(listIndividuos, true);
            string filePath = Path.Combine(Application.persistentDataPath, "individuos.json");
            
            File.WriteAllText(filePath, json);
            Debug.Log($"Saved to: {filePath}");
        }

        void SeleccionImagen(ClickEvent evt)
        {
            VisualElement caja = evt.currentTarget as VisualElement;
            imagenSelec = caja.resolvedStyle.backgroundImage;
            CajasBordeNegro();
            CajasBordeRojo(caja);

            if (toggleModificar.value){
                individuoSelec.Imagen = imagenSelec;
            }
        }

        private void LoadImages(ref List<VisualElement> imagenes)
        {
            imagenes.ForEach(caja => 
                {
                var tex = Resources.Load<Texture2D>($"{caja.name}");
                caja.style.backgroundImage = new StyleBackground(tex);
                });
        }

        void CajasBordeNegro()
        {
            imagenes.ForEach(elem =>
                {
                    elem.style.borderBottomColor = Color.black;
                    elem.style.borderRightColor = Color.black;
                    elem.style.borderTopColor = Color.black;
                    elem.style.borderLeftColor = Color.black;
                });
        }

        void CajasBordeRojo(VisualElement caja)
        {
            caja.style.borderBottomColor = Color.red;
            caja.style.borderRightColor = Color.red;
            caja.style.borderTopColor = Color.red;
            caja.style.borderLeftColor = Color.red;
        }

    }
}
