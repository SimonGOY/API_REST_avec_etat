using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API_REST_avec_état.Models.EntityFramework
{
    [Table("serie")]
    public partial class Serie
    {

        public Serie(int _serieid, string _titre, string _resume, int _nbsaisons, int _nbepisodes, int _anneecreation, string _network)
        {
            this.Serieid = _serieid;
            this.Titre = _titre;
            this.Resume = _resume;
            this.Nbsaisons = _nbsaisons;
            this.Nbepisodes = _nbepisodes;
            this.Anneecreation = _anneecreation;
            this.Network = _network;
        }

        public Serie():this(0,"","", 0,0,0,"") {}

        [Key]
        [Column("serieid")]
        public int Serieid { get; set; }
        [Column("titre")]
        [StringLength(100)]
        public string Titre { get; set; } = null!;
        [Column("resume")]
        public string? Resume { get; set; }
        [Column("nbsaisons")]
        public int? Nbsaisons { get; set; }
        [Column("nbepisodes")]
        public int? Nbepisodes { get; set; }
        [Column("anneecreation")]
        public int? Anneecreation { get; set; }
        [Column("network")]
        [StringLength(50)]
        public string? Network { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Serie serie &&
                   Serieid == serie.Serieid &&
                   Titre == serie.Titre &&
                   Resume == serie.Resume &&
                   Nbsaisons == serie.Nbsaisons &&
                   Nbepisodes == serie.Nbepisodes &&
                   Anneecreation == serie.Anneecreation &&
                   Network == serie.Network;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Serieid, Titre, Resume, Nbsaisons, Nbepisodes, Anneecreation, Network);
        }
    }


}
