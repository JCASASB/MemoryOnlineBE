using MemoryOnline.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOnline.Domain.Entities
{
    public class Card
    {
        // Propiedades con private set para proteger la inmutabilidad
        public Guid Id { get; private set; }
        public int Value { get; private set; }
        public string ImgUrl { get; private set; }
        public CardState State { get; private set; }

        // Constructor privado para EF Core y el Builder
        private Card() { }

        // Métodos de negocio

        public void Flip()
        {
            if (State == CardState.FaceDown)
                State = CardState.FaceUp;
            else if (State == CardState.FaceUp)
                State = CardState.FaceDown;
        }

        public void MarkAsMatched() => State = CardState.Matched;

        // Clase Builder interna
        public class Builder
        {
            private readonly Card _card = new Card();

            public Builder()
            {
                // Valores por defecto iniciales
                _card.Id = Guid.NewGuid();
                _card.State = CardState.FaceDown;
            }

            public Builder WithId(Guid id) { _card.Id = id; return this; }
            public Builder WithValue(int value) { _card.Value = value; return this; }
            public Builder WithImage(string image) { _card.ImgUrl = image; return this; }
            public Builder WithState(CardState state) { _card.State = state; return this; }

            public Card Build()
            {
                // Validación básica
                if (string.IsNullOrEmpty(_card.ImgUrl))
                    throw new InvalidOperationException("La carta debe tener una imagen asignada.");

                return _card;
            }
        }
    }
}
