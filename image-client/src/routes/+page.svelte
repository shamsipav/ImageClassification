<script lang="ts">
    import { fade } from "svelte/transition";

    let files: FileList;

    let message = "";

    let result: any;
    
    let loading = false;

    let errorText = "";
    
    const handleSubmit = async () => {
        result = null;

        if (files == null) return;

        errorText = "";

        if (!files[0].type.startsWith('image/')) {
            errorText = "Загружать можно только изображения"
            return;
        }

        loading = true;

        const formData = new FormData();
        formData.append("file", files[0]);
        
        try {
            const response = await fetch('http://93.88.178.186:5444/api/image/predict', {
                method: 'POST',
                body: formData
            });
            
            if (response.ok) {
                result = await response.json();
                console.log("Файл успешно отправлен на сервер");
            } else {
                errorText = await response.text();
                console.error("Что-то пошло не так при отправке файла");
            }

            loading = false;

        } catch (error) {
            errorText = "Возникла ошибка при обработке файла";
            loading = false;
            console.error("Ошибка при отправке файла: ", error);
        }
    }

    const translateResult = (label: string) => {
        if (label === "welding_line") {
            return "Линия сварки";
        } else if (label === "punching_hole") {
            return "Пробивка";
        } else if (label === "oil_spot") {
            return "Масляное пятно";
        } else if (label === "waist folding") {
            return "Поясная складка";
        } else if (label === "crease") {
            return "Складка";
        } else if (label === "crescent_gap") {
            return "Серповидный зазор";
        } else if (label === "rolled_pit") {
            return "Прокатная ямка";
        } else if (label === "water_spot") {
            return "Водяное пятно";
        } else if (label === "inclusion") {
            return "Неметаллическое включение";
        } else if (label === "silk_spot") {
            return "Шелковое пятно";
        } else {
            return "";
        }
    }
</script>

<div class="container">
    <div class="main">
        <h1>Распознавание дефектов на металлической поверхности</h1>

        <div class="form">
            <input type="file" bind:files />
            <button on:click={handleSubmit}>Распознать деффекты</button>
        </div>

        <div class="loader-box">
            {#if loading}
            <div class="loader" transition:fade>
                <svg version="1.1" id="L9" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                  viewBox="0 0 100 100" enable-background="new 0 0 0 0" xml:space="preserve">
                    <path fill="#000" d="M73,50c0-12.7-10.3-23-23-23S27,37.3,27,50 M30.9,50c0-10.5,8.5-19.1,19.1-19.1S69.1,39.5,69.1,50">
                      <animateTransform 
                         attributeName="transform" 
                         attributeType="XML" 
                         type="rotate"
                         dur="1s" 
                         from="0 50 50"
                         to="360 50 50" 
                         repeatCount="indefinite" />
                  </path>
                </svg>
            </div>
        {/if}
        </div>

        <div class="result">
            {#if result}
                <p class="result-header">Результат распознавания дефектов:</p>
                {#each result as item, i}
                    <p class="item" transition:fade>{translateResult(item.key)} ({item.key}) {item.value.toFixed(3)}</p>
                {/each}
            {/if}
        </div>

        <p class="error">{errorText}</p>
    </div>
    <div class="footer">
        <p>Практическая работа №1 по дисциплине "Технологии и инструменты машинного обучения"</p>
        <p>НМТМ-223901 | Шамсимухаметов П.Р.</p>
    </div>
</div>

<style>
    h1 {
        padding-top: 10rem;
        font-size: 2rem;
        font-weight: 300;
        text-align: center;
        margin-bottom: 4rem;
    }

    button {
        background-color: none;
        border: 1px solid black;
        border-radius: 0.2rem;
        padding: 0.2rem 0.5rem;
        transition: 0.2s ease;
    }

    button:hover {
        background-color: black;
        color: white;
    }

    .form {
        width: fit-content;
        margin: 0 auto;
    }

    .footer {
        margin-top: 6rem;
        text-align: center;
    }

    .footer p {
        line-height: 150%;
    }

    .loader-box {
        height: 5rem;
    }

    .loader {
        margin: 0 auto;
        width: 5rem;
    }

    .result {
        text-align: center;
        height: 19rem;
    }

    .item {
        margin-top: 0.3rem;
    }

    .result-header {
        font-weight: 600;
        margin-bottom: 1rem;
    }

    .error {
        text-align: center;
        font-weight: 300;
        font-size: 1.1rem;
        color: rgba(231, 61, 61, 0.671);
    }
</style>