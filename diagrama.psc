Algoritmo diagrama
	Definir cant, i, op Como Entero
	Definir titulo, autor, nombreLibro, busq, estado Como Cadena
	Dimension  titulos[100], autores[100], estados[100]
	
	cant<-0
	
	Escribir "MENÚ DE BIBLIOTECA"
	Escribir "1.  Registrar libros"
	Escribir "2. Consultar catálogo de libros"
	Escribir "3. Buscar libros por título o autor"
	Escribir "4. Registrar préstamos"
	Escribir "5. Registrar devolución"
	Escribir "5. Mostrar libros disponibles"
	Escribir "7. Mostrar libros disponibles y prestados"
	Escribir "8. Salir"
	Escribir  "Inserta la opción que deseas realizar"
	Leer op
	
	Segun op Hacer
		1:
			Si cant<100 Entonces
				cant<- cant+1
				Escribir "Título del libro"
				Leer titulos[cant]
				Escribir "Autor del libro"
				Leer autores[cant]
				estados[cant]<-"Disponible"
				Escribir "Libro registrado, guardandno..."
			SiNo
				Escribir "No hay suficiente espacio"
			FinSi
		
		
		2:
			Si cant = 0 Entonces
				Escribir "No hay libros registrados."
			SiNo
				Escribir "Catálogo"
				Para i <- 1 Hasta cant Hacer
					Escribir i, " ", titulos[i], "  ", autores[i], " [", estados[i], "]"
				FinPara
			FinSi
			Si  encontrado=0 Entonces
				
			FinSi
			
		3:
			Escribir "Escribe el titulo o autor a buscar:"
				Leer busq
				encontrado<-0
				Para i<-1 Hasta cant Hacer
					Si titulos[i] = busq O autores[i] = busq Entonces
						Escribir "Encontrado: ", titulos[i], " (", autores[i], ") - ", estados[i]
						encontrado<-1
					FinSi
				FinPara
				Si encontrado = 0 Entonces
					Escribir "No se encontraron coincidencias."
				FinSi
			
			
		4:
			Escribir "Escribe el titulo del libro a prestar:"
			Leer nombreLibro
			encontrado<-0
			Para i<-1 Hasta cant Hacer
				Si titulos[i] = nombreLibro Entonces
					encontrado<-1
					Si estados[i] = "Disponible" Entonces
						estados[i] <- "Prestado"
						Escribir "Guardando archivo..."
					SiNo
						Escribir "El libro no esta disponible ya ha sido prestado"
					FinSi
				FinSi
			FinPara
			Si encontrado = 0 Entonces
				Escribir "Ese libro no existe"
			FinSi
			
			
			
		5:
			Escribir "Escribe el título del libro que deseas devolver"
			Leer nombreLibro
			encontrado<-0
			Para i<-1 Hasta cant Hacer
				Si titulos[i]=nombreLibro Entonces
					estados[i]<- "Disponible"
					Escribir "¡Devolución registrada!"
				SiNo
					Escribir "El libro seleccionado no estaba prestado"
				FinSi
			FinPara
			
			
		6:
			Escribir "Libros disponibles"
			Para i<-1 Hasta cant Hacer
				Si estados[i] = "Disponible" Entonces
					Escribir " ", titulos[i], " (", autores[i], ")"
				FinSi
			FinPara
			Escribir "Libros prestados"
			Para i<-1 Hasta cant Hacer
				Si estados[i] = "Prestado" Entonces
					Escribir "  ", titulos[i], " (", autores[i], ")"
				FinSi
			FinPara
			
			
		7: 
			Escribir "Guardando archivos"
			Escribir "Finalizando el programa...."
	Fin Segun
	
FinAlgoritmo
