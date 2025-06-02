function mostrarPista(id) {
    document.getElementById(id).style.display = "flex";
}

function cerrarPista(id) {
    document.getElementById(id).style.display = "none";
}

document.addEventListener("DOMContentLoaded", () => {
    //Sala 3
    const sala3 = document.getElementById("sala3");
    if (sala3) {
        const mensaje = document.getElementById("mensaje");
        
        // Inicialización del numpad (siempre debe estar disponible si el QTE está completado)
        const display = document.getElementById("pad-display");
        const numKeys = document.querySelectorAll(".num-key");
        const clearButton = document.getElementById("clear");
        const submitButton = document.getElementById("submit");
        const padbtn = document.getElementById("padbtn");

        if (display && numKeys.length > 0) {
            numKeys.forEach((key) => {
                key.addEventListener("click", () => {
                    if (display.value.length < 5) {
                        display.value += key.getAttribute("data-value");
                    }
                });
            });

            clearButton?.addEventListener("click", () => {
                display.value = "";
            });

            padbtn?.addEventListener("click", () => {
                document.getElementById("pad").style.display = "block";
            });

            submitButton?.addEventListener("click", () => {
                const inputValue = display.value;
                if (inputValue && inputValue.length === 5) {
                    const form = document.createElement("form");
                    form.method = "POST";
                    form.action = "/Home/Sala3";
                    
                    const input = document.createElement("input");
                    input.type = "hidden";
                    input.name = "clave";
                    input.value = inputValue;
                    
                    form.appendChild(input);
                    document.body.appendChild(form);
                    form.submit();
                } else {
                    alert('Por favor, ingrese un número válido de 5 dígitos');
                }
            });
        }

        // Si el QTE está completado, mostrar estado final y salir
        if (qteTerminado === true) {
            mensaje.textContent = "¡Evento completado!";
            sala3.style.backgroundImage = "url('/images/fondoSala35.png')";
            document.getElementById("padbtn").style.display = "inline-block";
            return; // Solo salimos de la parte del QTE
        }

        // Código del QTE
        const fases = [
            { tecla: "w", imagen: "/images/fondoSala30.png", mensaje: "¡Fase 1! Spam W lo más rápido posible", tiempoLimite: 3000 },
            { tecla: "s", imagen: "/images/fondoSala31.png", mensaje: "¡Fase 2! Spam S lo más rápido posible", tiempoLimite: 3500 },
            { tecla: "a", imagen: "/images/fondoSala32.png", mensaje: "¡Fase 3! Spam A lo más rápido posible", tiempoLimite: 4000 },
            { tecla: "d", imagen: "/images/fondoSala33.png", mensaje: "¡Fase Final! Spam D lo más rápido posible", tiempoLimite: 4500 }
        ];

        const teclas = {
            w: document.getElementById("teclaW"),
            s: document.getElementById("teclaS"),
            a: document.getElementById("teclaA"),
            d: document.getElementById("teclaD")
        };

        let faseActual = 0;
        let pulsaciones = 0;
        let faseEnCurso = false;
        let tiempoInicio = 0;
        let temporizador = null;
        const pulsacionesRequeridas = 15;
        const pausaEntreFases = 2000;

        function destacarTecla(tecla) {
            Object.values(teclas).forEach(el => el.classList.remove("destacar"));
            if (teclas[tecla]) {
                teclas[tecla].classList.add("destacar");
            }
        }

        function iniciarFase() {
            if (faseActual < fases.length) {
                mensaje.textContent = "Preparando siguiente fase...";
                faseEnCurso = false;
                destacarTecla(null);

                setTimeout(() => {
                    pulsaciones = 0;
                    faseEnCurso = true;
                    tiempoInicio = Date.now();

                    const fase = fases[faseActual];
                    sala3.style.backgroundImage = `url('${fase.imagen}')`;
                    mensaje.textContent = `${fase.mensaje} (0/${pulsacionesRequeridas})`;
                    destacarTecla(fase.tecla);

                    clearTimeout(temporizador);
                    temporizador = setTimeout(() => {
                        if (faseEnCurso) {
                            faseEnCurso = false;
                            if (pulsaciones < pulsacionesRequeridas) {
                                mensaje.textContent = "¡Fallaste! Inténtalo de nuevo.";
                                sala3.style.backgroundImage = "url('/images/fondoSala30.png')";
                                destacarTecla(null);
                                faseActual = 0;
                                setTimeout(iniciarFase, pausaEntreFases);
                            }
                        }
                    }, fase.tiempoLimite);
                }, pausaEntreFases);
            }
        }

        document.addEventListener("keydown", (event) => {
            if (!faseEnCurso) return;

            const fase = fases[faseActual];
            const teclaPresionada = event.key.toLowerCase();

            if (teclaPresionada === fase.tecla) {
                pulsaciones++;
                mensaje.textContent = `${fase.mensaje} (${pulsaciones}/${pulsacionesRequeridas})`;

                if (pulsaciones >= pulsacionesRequeridas) {
                    faseEnCurso = false;
                    clearTimeout(temporizador);

                    if (faseActual === fases.length - 1) {
                        // Última fase completada
                        setTimeout(() => {
                            mensaje.textContent = "¡Evento completado!";
                            sala3.style.backgroundImage = "url('/images/fondoSala35.png')";
                            destacarTecla(null);
                            document.getElementById("padbtn").style.display = "inline-block";
                            fetch('/Home/CompletarQte', { method: 'POST' })
                                .then(() => window.location.reload());
                        }, 50);
                    } else {
                        faseActual++;
                        destacarTecla(null);
                        iniciarFase();
                    }
                }
            } else {
                faseEnCurso = false;
                clearTimeout(temporizador);
                mensaje.textContent = "¡Tecla incorrecta! Inténtalo de nuevo.";
                sala3.style.backgroundImage = "url('/images/fondoSala30.png')";
                destacarTecla(null);
                faseActual = 0;
                setTimeout(iniciarFase, pausaEntreFases);
            }
        });

        iniciarFase();

        //Sala 4
        if (document.querySelector(".sala4-container")) {
            const zonaJuego = document.getElementById("zonaJuego");
            const bate = document.getElementById("bate");
            const contenedor = document.querySelector(".contenedor");

            const cantidadRatitas = 5;
            const numeroMostrar = "40647";
            let indiceDigito = 0;
            let bateMoviendose = false;

            function crearRatita() {
                const ratita = document.createElement("div");
                ratita.className = "ratita";
                
                // Posición aleatoria dentro de la zona de juego
                const maxX = zonaJuego.clientWidth - 80;
                const maxY = zonaJuego.clientHeight - 80;
                const x = Math.floor(Math.random() * maxX);
                const y = Math.floor(Math.random() * maxY);
                
                ratita.style.left = x + "px";
                ratita.style.top = y + "px";
                
                ratita.addEventListener("click", (e) => golpearRatita(e, ratita));
                zonaJuego.appendChild(ratita);
                
                // Mover la ratita cada cierto tiempo
                setInterval(() => {
                    if (!ratita.classList.contains("golpeada")) {
                        const newX = Math.floor(Math.random() * maxX);
                        const newY = Math.floor(Math.random() * maxY);
                        ratita.style.left = newX + "px";
                        ratita.style.top = newY + "px";
                    }
                }, 2000);
            }

            function golpearRatita(evento, ratita) {
                if (ratita.classList.contains("golpeada") || bateMoviendose) return;
                
                bateMoviendose = true;
                
                // Posicionar el bate donde se hizo clic
                const rect = contenedor.getBoundingClientRect();
                const x = evento.clientX - rect.left;
                const y = evento.clientY - rect.top;
                
                bate.style.left = (x - 45) + "px";
                bate.style.top = (y - 90) + "px";
                bate.style.transform = "rotate(-45deg)";
                
                // Animar el golpe
                setTimeout(() => {
                    bate.style.transform = "rotate(0deg)";
                    ratita.classList.add("golpeada");
                    
                    // Mostrar número
                    const numero = document.createElement("div");
                    numero.className = "numero";
                    numero.textContent = numeroMostrar[indiceDigito];
                    numero.style.left = (x - 10) + "px";
                    numero.style.top = (y - 20) + "px";
                    contenedor.appendChild(numero);
                    
                    // Desvanecer número
                    setTimeout(() => {
                        numero.remove();
                    }, 1000);
                    
                    indiceDigito++;
                    bateMoviendose = false;
                    
                    // Verificar si se han golpeado todas las ratas
                    if (document.querySelectorAll(".ratita.golpeada").length === cantidadRatitas) {
                        setTimeout(() => {
                            alert("¡Has encontrado todos los números! El código es: " + numeroMostrar);
                            mostrarFormularioClave(numeroMostrar);
                        }, 1000);
                    }
                }, 200);
            }

            // Crear las ratitas iniciales
            for (let i = 0; i < cantidadRatitas; i++) {
                crearRatita();
            }
        }
    }
});