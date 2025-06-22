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
            document.getElementById("qte").style.display = "none";
            sala3.style.backgroundImage = "url('/images/fondoSala34.png')";
            document.getElementById("padbtn").style.display = "inline-block";

        }

        // Código del QTE
        const fases = [
            { tecla: "w", imagen: "/images/fondoSala30.png", mensaje: "Spammea", tiempoLimite: 3000 },
            { tecla: "s", imagen: "/images/fondoSala31.png", mensaje: "Spammea", tiempoLimite: 3500 },
            { tecla: "a", imagen: "/images/fondoSala32.png", mensaje: "Spammea", tiempoLimite: 4000 },
            { tecla: "d", imagen: "/images/fondoSala33.png", mensaje: "Spammea", tiempoLimite: 4500 }
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
                    destacarTecla(fase.tecla);

                    clearTimeout(temporizador);
                    temporizador = setTimeout(() => {
                        if (faseEnCurso) {
                            faseEnCurso = false;
                            if (pulsaciones < pulsacionesRequeridas) {
                                mensaje.textContent = "¡Fallaste! Inténtalo de nuevo.";
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

            if (teclaPresionada == fase.tecla) {
                pulsaciones++;
                mensaje.textContent = `${fase.mensaje}`;

                if (pulsaciones >= pulsacionesRequeridas) {
                    faseEnCurso = false;
                    clearTimeout(temporizador);

                    if (faseActual === fases.length - 1) {
                        // Última fase completada
                        setTimeout(() => {
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
            }else{
                faseEnCurso = false;
                clearTimeout(temporizador);
                mensaje.textContent = "¡Tecla incorrecta! Inténtalo de nuevo.";
                destacarTecla(null);
                faseActual = 0;
                setTimeout(iniciarFase, pausaEntreFases);
            }
        });

        iniciarFase();
    }

    // Sala 4
    const sala4 = document.querySelector('.sala4');
    if (sala4) {
        console.log('Inicializando Sala 4...');
        
        const areaJuego = sala4.querySelector('.area-juego');
        const bate = sala4.querySelector('.bate');

        if (!areaJuego || !bate) {
            console.error('No se encontraron elementos necesarios para la Sala 4');
            return;
        }

        const CODIGO = '40647';
        const NUM_RATAS = 5;
        let ratasGolpeadas = 0;
        let digitoActual = 0;
        let animacionEnProgreso = false;

        function crearRatas() {
            console.log('Creando ratas...');
            for (let i = 0; i < NUM_RATAS; i++) {
                const rata = document.createElement('div');
                rata.className = 'rata';
                posicionarRataAleatoriamente(rata);
                rata.addEventListener('click', (e) => golpearRata(e, rata));
                areaJuego.appendChild(rata);
                console.log('Rata creada:', i + 1);

                setInterval(() => {
                    if (!rata.classList.contains('golpeada')) {
                        posicionarRataAleatoriamente(rata);
                    }
                }, Math.random() * 900 + 900);
            }
        }

        function posicionarRataAleatoriamente(rata) {
            const maxX = areaJuego.clientWidth - 60;
            const maxY = areaJuego.clientHeight - 60;
            const x = Math.random() * maxX;
            const y = Math.random() * maxY;
            
            rata.style.left = x + 'px';
            rata.style.top = y + 'px';
        }

        function golpearRata(evento, rata) {
            if (rata.classList.contains('golpeada') || animacionEnProgreso) return;
            
            console.log('Golpeando rata...');
            animacionEnProgreso = true;
            
            const rect = areaJuego.getBoundingClientRect();
            const x = evento.clientX - rect.left;
            const y = evento.clientY - rect.top;

            bate.style.left = (x - 40) + 'px';
            bate.style.top = (y - 160) + 'px';
            bate.style.display = 'block';
            bate.style.transform = 'rotate(-45deg)';

            setTimeout(() => {
                bate.style.transform = 'rotate(0deg)';
                rata.classList.add('golpeada');

                const numero = document.createElement('div');
                numero.className = 'numero';
                numero.textContent = CODIGO[digitoActual];
                numero.style.left = (x - 15) + 'px';
                numero.style.top = (y - 15) + 'px';
                areaJuego.appendChild(numero);

                setTimeout(() => {
                    numero.remove();
                    bate.style.display = 'none';
                }, 1000);

                digitoActual++;
                ratasGolpeadas++;
                animacionEnProgreso = false;

                console.log('Ratas golpeadas:', ratasGolpeadas);
                if (ratasGolpeadas === NUM_RATAS) {
                    setTimeout(() => {
                        mostrarFormularioClave(CODIGO);
                    }, 1000);
                }
            }, 200);
        }

        // Iniciar el juego
        setTimeout(crearRatas, 500); // Dar tiempo a que todo se cargue correctamente
    }
});