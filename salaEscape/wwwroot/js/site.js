function mostrarPista(id) {
    document.getElementById(id).style.display = "flex";
}

function cerrarPista(id) {
    document.getElementById(id).style.display = "none";
}
const sala3 = document.getElementById("sala3");
const mensaje = document.getElementById("mensaje");

const fases = [
    { tecla: "ArrowUp", imagen: "URL_DE_TU_IMAGEN_AQUI", mensaje: "¡Fase 1! Spam ↑ lo más rápido posible", tiempoLimite: 3000 },
    { tecla: "ArrowDown", imagen: "URL_DE_TU_IMAGEN_AQUI", mensaje: "¡Fase 2! Spam ↓ lo más rápido posible", tiempoLimite: 3500 },
    { tecla: "ArrowLeft", imagen: "URL_DE_TU_IMAGEN_AQUI", mensaje: "¡Fase 3! Spam ← lo más rápido posible", tiempoLimite: 4000 },
    { tecla: "ArrowRight", imagen: "URL_DE_TU_IMAGEN_AQUI", mensaje: "¡Fase Final! Spam → lo más rápido posible", tiempoLimite: 4500 }
];

const flechas = {
    ArrowUp: document.getElementById("flechaUp"),
    ArrowDown: document.getElementById("flechaDown"),
    ArrowLeft: document.getElementById("flechaLeft"),
    ArrowRight: document.getElementById("flechaRight")
};

let faseActual = 0;
let pulsaciones = 0;
let faseEnCurso = false;
let tiempoInicio = 0;
let temporizador = null;
const pulsacionesRequeridas = 15;
const pausaEntreFases = 2000;

function destacarFlecha(tecla) {
    Object.values(flechas).forEach(el => el.classList.remove("destacar"));
    if (flechas[tecla]) {
        flechas[tecla].classList.add("destacar");
    }
}

function iniciarFase() {
    if (faseActual < fases.length) {
        mensaje.textContent = "Preparando siguiente fase...";
        sala3.style.backgroundImage = "none";
        faseEnCurso = false;
        destacarFlecha(null);

        setTimeout(() => {
            pulsaciones = 0;
            faseEnCurso = true;
            tiempoInicio = Date.now();

            const fase = fases[faseActual];
            sala3.style.backgroundImage = `url('${fase.imagen}')`;
            mensaje.textContent = `${fase.mensaje} (0/${pulsacionesRequeridas})`;
            destacarFlecha(fase.tecla);

            clearTimeout(temporizador);
            temporizador = setTimeout(() => {
                if (faseEnCurso) {
                    faseEnCurso = false;
                    if (pulsaciones < pulsacionesRequeridas) {
                        mensaje.textContent = "¡Fallaste! Inténtalo de nuevo.";
                        sala3.style.backgroundImage = "none";
                        destacarFlecha(null);
                        faseActual = 0;
                        setTimeout(iniciarFase, pausaEntreFases);
                    }
                }
            }, fase.tiempoLimite);
        }, pausaEntreFases);
    } else {
        mensaje.textContent = "¡Evento completado!";
        sala3.style.backgroundImage = "url('')";
        destacarFlecha(null);
        document.getElementById("padbtn").style.display = "inline-block";
    }
}

document.addEventListener("keydown", (event) => {
    if (!faseEnCurso) return;

    const fase = fases[faseActual];

    if (event.key === fase.tecla) {
        pulsaciones++;
        mensaje.textContent = `${fase.mensaje} (${pulsaciones}/${pulsacionesRequeridas})`;

        if (pulsaciones >= pulsacionesRequeridas) {
            faseEnCurso = false;
            clearTimeout(temporizador);
            faseActual++;
            destacarFlecha(null);
            iniciarFase();
        }
    } else {
        faseEnCurso = false;
        clearTimeout(temporizador);
        mensaje.textContent = "¡Tecla incorrecta! Inténtalo de nuevo.";
        sala3.style.backgroundImage = "none";
        destacarFlecha(null);
        faseActual = 0;
        setTimeout(iniciarFase, pausaEntreFases);
    }
});

iniciarFase();

const zonaJuego = document.getElementById("zonaJuego");
const bate = document.getElementById("bate");

const cantidadRatitas = 5;
const numeroMostrar = "40647";
let indiceDigito = 0;
let bateMoviendose = false;

function posicionAleatoria(max) {
    return Math.floor(Math.random() * max);
}

function crearRatita() {
    const ratita = document.createElement("div");
    ratita.className = "ratita";
    zonaJuego.appendChild(ratita);

    ratita.style.top = posicionAleatoria(zonaJuego.clientHeight - 80) + "px";
    ratita.style.left = posicionAleatoria(zonaJuego.clientWidth - 80) + "px";

    const mover = setInterval(() => {
        if (document.body.contains(ratita)) {
            ratita.style.top = posicionAleatoria(zonaJuego.clientHeight - 80) + "px";
            ratita.style.left = posicionAleatoria(zonaJuego.clientWidth - 80) + "px";
        } else {
            clearInterval(mover);
        }
    }, 700);

    ratita.style.transition = "top 0.7s ease, left 0.7s ease";

    ratita.addEventListener("click", function () {
        if (indiceDigito >= numeroMostrar.length) return;
        if (bateMoviendose) return;

        bateMoviendose = true;

        const rectRata = this.getBoundingClientRect();
        const rectContenedor = zonaJuego.getBoundingClientRect();

        const topBate = rectRata.top - rectContenedor.top;
        const leftBate = rectRata.left - rectContenedor.left;

        bate.style.right = "";
        bate.style.left = leftBate + "px";
        bate.style.top = topBate + "px";

        const onTransitionEnd = () => {
            bate.removeEventListener("transitionend", onTransitionEnd);
            bate.classList.add("agitar");

            const numero = document.createElement("div");
            numero.className = "numero";
            numero.innerText = numeroMostrar[indiceDigito];
            numero.style.top = topBate + "px";
            numero.style.left = leftBate + "px";
            zonaJuego.appendChild(numero);

            this.remove();
            indiceDigito++;

            setTimeout(() => {
                bate.classList.remove("agitar");
                bateMoviendose = false;

                if(indiceDigito >= cantidadRatitas) {
                    bate.style.transition = "opacity 1s ease";
                    bate.style.opacity = "0";

                    bate.addEventListener("transitionend", () => {
                        bate.style.display = "none";
                    }, { once: true });
                }
            }, 1500);
        };

        bate.addEventListener("transitionend", onTransitionEnd);
    });
}

for (let i = 0; i < cantidadRatitas; i++) {
    crearRatita();
}

const display = document.getElementById("pad-display");
const numKeys = document.querySelectorAll(".num-key");
const clearButton = document.getElementById("clear");
const submitButton = document.getElementById("submit");

numKeys.forEach((key) => {
    key.addEventListener("click", () => {
        if (display.value.length < 5) {
            display.value += key.getAttribute("data-value");
        }
    });
});

clearButton.addEventListener("click", () => {
    display.value = "";
});

document.getElementById("padbtn").addEventListener("click", () => {
    document.getElementById("pad").style.display = "block";
});

submitButton.addEventListener("click", () => {
    const inputValue = display.value;
    if (inputValue && inputValue.length === 5) {
        const data = { value: inputValue };
        fetch('/sala6', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });
    } else {
        alert('Por favor, ingrese un número válido de 5 dígitos');
    }
});